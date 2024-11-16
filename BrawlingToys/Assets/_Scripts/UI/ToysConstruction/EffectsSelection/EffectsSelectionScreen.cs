using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using Unity.Netcode;
using BrawlingToys.Actors;
using BrawlingToys.Managers;
using BrawlingToys.Core; 

namespace BrawlingToys.UI
{
    public class EffectsSelectionScreen : BaseScreen
    {
        [SerializeField]
        private GameObject _playerInfoPrefab;
        [SerializeField]
        private Transform _playerInfoHorizontalLayout;
        [SerializeField]
        //Variable created for prototype reasons only!
        private AssetReference _playerCharacterAssetRef;

        private ModifierScriptable _drawnEffect;
        [SerializeField]
        private List<Player> _players;

        [Header("DBs")]

        [SerializeField] private ModifiersDB _allModifiersDB;

        [Header("Subscreens")]

        [SerializeField] private GameObject _choiceScreen;
        [SerializeField] private GameObject _waitScreen;

        private List<PlayerInfoPanel> _currentPanels = new(); 
        private bool _panelsInstantiated = false;

        private int _serverCompletedSelectionClients;  

        private void Awake()
        {
            MatchManager.LocalInstance.OnPlayersSpawned += InitPlayers; 
        }

        public override void OnDestroy()
        {
            base.OnDestroy(); 
            MatchManager.LocalInstance.OnPlayersSpawned -= InitPlayers; 
        }
        
        protected override void OnScreenEnable()
        {
            Debug.Log("Enable"); 
            _serverCompletedSelectionClients = 0;
            DrawScreen();
        }

        private void InitPlayers()
        {
            if (NetworkManager.Singleton.IsHost && _players.Count == 0)
            {
                GetPlayersReferenceServerRpc();
            }
        }

        public void DrawScreen()
        {
            _waitScreen.SetActive(false);
            _choiceScreen.SetActive(true);
            
            for (int i = 0; i < _players.Count; i++)
            {
                var player = _players[i]; 

                Debug.Log(i);

                string playerName = "Player " + (player.PlayerId + 1);
                SpawnPlayerInfo(player, playerName, _playerCharacterAssetRef, new GameObject[0]);
            }

            _panelsInstantiated = true; 
        }

        public void SpawnPlayerInfo(Player player, string playerName, AssetReference characterAsset, GameObject[] effectsGo)
        {
            var playerInfo = _panelsInstantiated 
                ? GetPlayerPanel(player) 
                : GeneratePanel();

            playerInfo.FillInfoPanel(player, playerName, characterAsset, effectsGo);
            playerInfo.GetPlayerInfoClickEvent().AddListener(OnTargetSelected);
        }

        private PlayerInfoPanel GeneratePanel()
        {
            GameObject playerInfoGo = Instantiate(_playerInfoPrefab, _playerInfoHorizontalLayout);
            PlayerInfoPanel playerInfo = playerInfoGo.GetComponent<PlayerInfoPanel>();

            _currentPanels.Add(playerInfo); 

            return playerInfo; 
        }

        private PlayerInfoPanel GetPlayerPanel(Player player)
        {
            var panel = _currentPanels
                .First(p => p.Player.PlayerId == player.PlayerId);

            return panel;  
        }

        public void SetDrawnEffect(ModifierScriptable drawnEffect) => _drawnEffect = drawnEffect;

        public void OnTargetSelected(PlayerInfoPanel playerInfoPanel)
        {
            var playerSelected = playerInfoPanel.Player; 
            
            SetModifierToPlayerServerRpc(playerSelected.PlayerId, _drawnEffect.Tag);

            _waitScreen.SetActive(true);
            _choiceScreen.SetActive(false);

            ResgisterCompletitionServerRpc(); 
        }

        #region Online Actions

        [ServerRpc(RequireOwnership = false)]
        private void GetPlayersReferenceServerRpc()
        {
            GetPlayersReferenceClientRpc();
        }

        [ClientRpc]
        private void GetPlayersReferenceClientRpc()
        {
            foreach (var player in MatchManager.LocalInstance.MatchPlayers)
            {
                Debug.Log($"Register {player.PlayerId} in the screen"); 
                _players.Add(player);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetModifierToPlayerServerRpc(ulong playerID, string modifierTag)
        {
            SetModifierToPlayerClientRpc(playerID, modifierTag); 
        }

        [ClientRpc]
        private void SetModifierToPlayerClientRpc(ulong playerID, string modifierTag)
        {
            var player = MatchManager.LocalInstance.MatchPlayers
                .First(p => p.PlayerId == playerID);
            var modifier = _allModifiersDB.GetCurrentDataBase()
                .First(m => m.Tag.Equals(modifierTag)); 

            Debug.Log($"Adding modifier: {modifier.Tag} To {player.PlayerId}");
            player.Stats.Mediator.AddModifier(modifier); 
        }

        [ServerRpc(RequireOwnership = false)]
        private void ResgisterCompletitionServerRpc()
        {
            _serverCompletedSelectionClients++; 

            var totalClientsPlaying = MatchManager.LocalInstance.MatchPlayers.Length;
            var totalClientsFinished = _serverCompletedSelectionClients;

            Debug.Log($"Total clients finished: {totalClientsFinished} - Total clients player: {totalClientsPlaying}"); 

            if(totalClientsPlaying == totalClientsFinished)
            {
                CloseScreenClientRpc(); 
            }
        }

        [ClientRpc]
        private void CloseScreenClientRpc()
        {
            ScreenManager.instance.ToggleScreenByTag(ScreenName, false);
            _serverCompletedSelectionClients = 0; 
        }

        #endregion
    }
}