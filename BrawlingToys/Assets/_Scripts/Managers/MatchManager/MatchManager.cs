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
        private Dictionary<Player, PlayerRoundInfo> _playerMatchInfo = new();
        private int _deadPlayersCount = 0;

        public Player[] MatchPlayers { get 
        {            
            return _playerMatchInfo.Keys.ToArray();  
        } }

        public Dictionary<Player, PlayerRoundInfo> PlayerMatchInfo { get => _playerMatchInfo; }

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
            GameManager.LocalInstance.OnGameStateChange.AddListener(TryResetMatch); 
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

            //player.OnPlayerInitialize.AddListener(AddPlayerMatchInfo);
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

            Debug.Log($"Player: {player.PlayerId} was inited on server");
        }

        private void TryResetMatch(GameStateType newGameState)
        {
            if (newGameState == GameStateType.Combat && NetworkManager.Singleton.IsHost)
            {
                ResetMatchInfoServerRpc(); 
                
                var serializedIds = _playerMatchInfo
                    .Select(p => p.Key.PlayerId)
                    .ToArray(); 

                EnablePlayersServerRpc(); 
            }
        }

        public void AddPlayerMatchInfo(Player player)
        {
            _playerMatchInfo.Add(player, new PlayerRoundInfo(0, true));
        }

        public void RegisterKill(Player player)
        {
            Debug.Log($"Register kill, killer: {player}");
            if (player == null)
                return;

            _playerMatchInfo[player].KillsAmount++;
        }

        public void RegisterDeath(Player player)
        {
            Debug.Log("Register Death");
            _playerMatchInfo[player].IsSurvivor = false;

            _deadPlayersCount++;
            CheckMatchEnd();
        }

        private void CheckMatchEnd()
        {
            if(MatchIsEnded())
            {
                // foreach (Player player in _playerMatchInfo.Keys)
                // {
                //     player.OnPlayerInitialize.RemoveListener(AddPlayerMatchInfo);
                //     player.OnPlayerKill.RemoveListener(RegisterKill);
                //     player.OnPlayerDeath.RemoveListener(RegisterDeath);
                // }

                CallResultScreenServerRpc(); 
            }

            bool MatchIsEnded() => _playerMatchInfo.Count - _deadPlayersCount <= 1; 
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
            Debug.Log("Chaves: " + _playerMatchInfo.Keys.ToArray().Length);
            
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
