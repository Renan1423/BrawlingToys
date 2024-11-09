using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BrawlingToys.Network;
using BrawlingToys.Actors;
using BrawlingToys.Managers;
using Unity.Netcode;
using System;
using TMPro;

namespace BrawlingToys.UI
{
    public class WaitingForPlayersScreen : BaseScreen
    {
        [Space(20)]
        [Header("References")]
        [SerializeField]
        private JoinRoom _joinRoom;

        [Space(20)]

        [Header("Waiting For Players Screen")]
        [SerializeField]
        private List<WaitingPlayer> _waitingPlayerPanels;
        [SerializeField]
        private Button _playButton;
        [SerializeField]
        private TextMeshProUGUI _partyCodeText;

        private int _playersCount;

        protected override void OnScreenEnable()
        {
            RemoveAllPlayers();
        }

        public void InitializeWaitingRoom(string partyCode, string hostPlayerName)
        {
            if (!IsServer)
                return;

            ValidadeMatch();

            _partyCodeText.text = "C�digo da sala: " + partyCode;
            NetworkManager.Singleton.OnClientConnectedCallback += OnNewClientConnected;
            _joinRoom.OnNewPlayerJoined += OnNewPlayerJoined;
            AddPlayer(hostPlayerName);
        }

        protected override void CloseScreen(float delayToClose)
        {
            base.CloseScreen(delayToClose);

            NetworkManager.Singleton.OnClientConnectedCallback -= OnNewClientConnected;
            _joinRoom.OnNewPlayerJoined -= OnNewPlayerJoined;
        }

        private void OnNewClientConnected(ulong clientId) 
        {
            if (NetworkManager.Singleton.IsHost) 
            {
                var client = NetworkManager.Singleton.ConnectedClients.GetValueOrDefault(clientId);
                var playerPref = client.PlayerObject;

                List<PlayerClientData> playerClientDatas = new List<PlayerClientData>(FindObjectsOfType<PlayerClientData>());

                PlayerClientData playerClientData = playerClientDatas.Find(cd => cd.PlayerID == clientId);

                playerPref.gameObject.SetActive(false);

                OnNewPlayerJoined(playerClientData);
            }
        }

        public void OnNewPlayerJoined(PlayerClientData playerClientData) 
        {
            PlayerClientDatasManager.LocalInstance.AddPlayerClientData(playerClientData);

            if (playerClientData != null)
                AddPlayer(playerClientData.PlayerUsername);
        }

        public void AddPlayer(string playerName) 
        {
            _waitingPlayerPanels[_playersCount].AddPlayer(_playersCount + 1, playerName);
            _playersCount++;
            ValidadeMatch();
        }

        public void RemovePlayer(int index) 
        {
            _waitingPlayerPanels[index].ResetPlayer();
            _playersCount--;

            //Turning, for example, the player 3 into the number 2 if the number 2 quit the match
            if (_waitingPlayerPanels[_playersCount + 1].HasPlayer) 
            {
                string playerName = _waitingPlayerPanels[_playersCount].PlayerName;

                _waitingPlayerPanels[_playersCount].AddPlayer(_playersCount, playerName);
                RemovePlayer(_playersCount + 1);
            }
        }

        private void RemoveAllPlayers() 
        {
            foreach (WaitingPlayer waitingPlayer in _waitingPlayerPanels)
            {
                waitingPlayer.ResetPlayer();
            }
        }

        private void ValidadeMatch() 
        {
            _playButton.interactable = (_playersCount > 1);
        }

        public void PlayGame() 
        {
            PlayGameServerRpc();
            LevelManager.LoadNextLevelNetwork();
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayGameServerRpc() 
        {
            PlayerGameClientRpc();
        }

        [ClientRpc]
        private void PlayerGameClientRpc() 
        {
            LevelManager.StartNetworkSceneTransition();
        }
    }
}
