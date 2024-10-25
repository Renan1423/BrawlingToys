using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BrawlingToys.Actors;
using BrawlingToys.Managers;
using Unity.Netcode;
using System.Linq;
using UnityEngine.InputSystem;

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

        protected override void OnScreenEnable()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                GetPlayersReferenceServerRpc();
            }
        }


        public void DrawScreen()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                string playerName = "Player " + i;
                SpawnPlayerInfo(_drawnEffect, _players[i].Stats, playerName, _playerCharacterAssetRef, new GameObject[0]);
            }
        }

        public void SpawnPlayerInfo(ModifierScriptable effectToApply, Stats playerStats, string playerName, AssetReference characterAsset, GameObject[] effectsGo)
        {
            GameObject playerInfoGo = Instantiate(_playerInfoPrefab, _playerInfoHorizontalLayout);
            PlayerInfoPanel playerInfo = playerInfoGo.GetComponent<PlayerInfoPanel>();

            playerInfo.FillInfoPanel(effectToApply, playerStats, playerName, characterAsset, effectsGo);
            playerInfo.GetPlayerInfoClickEvent().AddListener(OnTargetSelected);
        }

        public void SetDrawnEffect(ModifierScriptable drawnEffect) => _drawnEffect = drawnEffect;

        public void OnTargetSelected(PlayerInfoPanel playerInfoPanel)
        {
            ScreenManager.instance.ToggleScreenByTag(ScreenName, false);
        }

        #region Online Actions

        [ServerRpc(RequireOwnership = false)]
        private void GetPlayersReferenceServerRpc()
        {
            var playersIds = MatchManager.LocalInstance.MatchPlayers
                .Select(p => p.PlayerId)
                .ToArray();

            GetPlayersReferenceClientRpc(playersIds);
        }

        [ClientRpc]
        private void GetPlayersReferenceClientRpc(ulong[] playersIds)
        {
            foreach (var id in playersIds)
            {
                if (NetworkManager.Singleton.ConnectedClients.ContainsKey(id))
                {
                    var playerNetworkObject = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
                    var playerReference = playerNetworkObject.GetComponent<Player>();

                    _players.Add(playerReference);
                }
            }

            DrawScreen();
        }

        #endregion
    }
}