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
        //Variable created for prototype reasons only!
        private Stats _playerStats;

        [Header("Inputs")]

        [SerializeField] private InputActionAsset _inputActionAsset;
        private InputActionMap _gameplayMap;

        [Header("DBs")]

        [SerializeField] private ModifiersDB _allModifiersDB;

        private List<PlayerInfoPanel> _currentPanels = new(); 
        private bool _panelsInstantiated = false; 

        protected override void OnScreenEnable()
        {
            if (NetworkManager.Singleton.IsHost && _players.Count == 0)
            {
                GetPlayersReferenceServerRpc();
            }

            DrawScreen(); 
        }

        public void DrawScreen()
        {
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
            
            ScreenManager.instance.ToggleScreenByTag(ScreenName, false);
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

        #endregion
    }
}