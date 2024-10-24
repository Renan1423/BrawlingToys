using System;
using System.Collections.Generic;
using System.Linq;
using BrawlingToys.Actors;
using BrawlingToys.Network;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Managers
{
    public class MatchManager : NetworkSingleton<MatchManager>
    {
        [SerializeField]
        private GameObject _playerPrefab; 
        
        private Dictionary<Player, PlayerRoundInfo> _playerMatchInfo = new();
        private int _deadPlayersCount = 0;

        public Player[] MatchPlayers { get 
        {            
            return _playerMatchInfo.Keys.ToArray();  
        } }

        public Dictionary<Player, PlayerRoundInfo> PlayerMatchInfo { get => _playerMatchInfo; }

        private bool _playersSpawned = false; 

        private void Start()
        {
            SubscribeEvents(); 
        }

        public override void OnDestroy()
        {
            UnsubscribeEvents(); 
            
            base.OnDestroy();
        }

        private void SubscribeEvents()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += UpdateMatchInfoDictionary; 
            
            GameManager.LocalInstance.OnGameStateChange.AddListener(TrySetupCombatState); 
        }

        private void UnsubscribeEvents()
        {
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= UpdateMatchInfoDictionary;
            }
        }

        private void UpdateMatchInfoDictionary(ulong clientId)
        {
            if(NetworkManager.Singleton.IsHost) AddClientPlayerToDic(clientId);
        }

        private void AddClientPlayerToDic(ulong clientId)
        {
            var client = NetworkManager.Singleton.ConnectedClients.GetValueOrDefault(clientId); 
            var playerPref = client.PlayerObject; 

            var player = playerPref.GetComponent<Player>(); 

            player.OnPlayerKill.AddListener(RegisterKill);
            player.OnPlayerDeath.AddListener(RegisterDeath);

            var roundInfo = new PlayerRoundInfo(0, true); 

            _playerMatchInfo.Add(player, roundInfo);

            var serializedIds = _playerMatchInfo
                    .Keys
                    .Select(p => p.PlayerId)
                    .ToArray();
            
            var serializedKills = _playerMatchInfo
                    .Values
                    .Select(i => i.KillsAmount)
                    .ToArray(); 
                
                var serializedSurvivals = _playerMatchInfo
                    .Values
                    .Select(i => i.IsSurvivor)
                    .ToArray();

            SyncMatchPlayersServerRpc(serializedIds, serializedKills, serializedSurvivals); 
        }

        private void TrySetupCombatState(GameStateType newGameState)
        {
            if (newGameState == GameStateType.Combat && NetworkManager.Singleton.IsHost)
            {
                if(!_playersSpawned) SpawnPlayerPrefsServerRpc(); 
                
                ResetMatchInfoServerRpc(); 
                EnablePlayersServerRpc(); 
            }
        }

        public void AddPlayerMatchInfo(Player player)
        {
            _playerMatchInfo.Add(player, new PlayerRoundInfo(0, true));
        }

        public void RegisterKill(Player player)
        {
            //Debug.Log($"Register kill, killer: {player.PlayerId}");
            if (player == null)
                return;

            _playerMatchInfo[player].KillsAmount++;
        }

        public void RegisterDeath(Player player)
        {
            Debug.Log($"Register Death, dead: {player.PlayerId}");
            _playerMatchInfo[player].IsSurvivor = false;

            _deadPlayersCount++;
            CheckMatchEnd();
        }

        private void CheckMatchEnd()
        {
            if(MatchIsEnded())
            {
                CallResultScreenServerRpc(); 
            }

            bool MatchIsEnded() => _playerMatchInfo.Count - _deadPlayersCount <= 1; 
        }

        [ServerRpc]
        private void SpawnPlayerPrefsServerRpc()
        {
            var clientIds = NetworkManager.Singleton.ConnectedClientsIds; 
            
            foreach (var clientId in clientIds)
            {
                var playerInstance = Instantiate(_playerPrefab);

                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            }

            _playersSpawned = true; 
        } 

        [ServerRpc(RequireOwnership = false)]
        private void CallResultScreenServerRpc()
        {
            CallResultScreenClientRpc(); 
        }

        [ClientRpc]
        private void CallResultScreenClientRpc()
        {
            ScreenManager.instance.ToggleScreenByTag("ResultScreen", true);
        } 

        [ServerRpc]
        private void ResetMatchInfoServerRpc()
        {
            _deadPlayersCount = 0;
            
            foreach (Player player in MatchPlayers)
            {
                _playerMatchInfo[player] = new PlayerRoundInfo(0, true);
            }
        }

        [ServerRpc]
        private void EnablePlayersServerRpc()
        {
            EnablePlayersClientRpc(); 
        }

        [ClientRpc]
        private void EnablePlayersClientRpc()
        {
            foreach (var player in MatchPlayers)
            {
                player.gameObject.SetActive(true); 
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SyncMatchPlayersServerRpc(ulong[] playerId, int[] serializedKills, bool[] serializedSurvivals)
        {
            SyncMatchPlayersClientRpc(playerId, serializedKills, serializedSurvivals); 
        }

        [ClientRpc]
        private void SyncMatchPlayersClientRpc(ulong[] playerId, int[] serializedKills, bool[] serializedSurvivals)
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                var players = GameObject.FindObjectsByType<Player>(FindObjectsSortMode.None);
                _playerMatchInfo = new(); 

                for (int i = 0; i < playerId.Length; i++)
                {
                    var player = players.First(p => p.PlayerId == playerId[i]); 
    
                    var roundInfo = new PlayerRoundInfo(serializedKills[i], serializedSurvivals[i]);
    
                    _playerMatchInfo.Add(player, roundInfo);  
                }
            }
        }
    }
}
