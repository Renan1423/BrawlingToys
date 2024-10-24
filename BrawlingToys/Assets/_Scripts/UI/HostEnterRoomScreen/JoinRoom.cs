using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;
using BrawlingToys.Network;
using BrawlingToys.Core;
using BrawlingToys.Actors;
using Unity.Netcode;
using System;

namespace BrawlingToys.UI
{
    public class JoinRoom : BaseScreen
    {
        [SerializeField]
        private CodeInputValidator _codeInputValidator;
        [SerializeField]
        private NameInputValidator _nameInputValidator;

        [SerializeField]
        private CharacterSelectionScreen _characterSelectionScreen;

        public event Action<PlayerClientData> OnNewPlayerJoined; 

        public void Join() 
        {
            _codeInputValidator.ValidateCode();

            if (_nameInputValidator.CheckNameValidation()) 
            {
                JoinParty(OnJoinParty, _codeInputValidator.InputFieldText);
            }
        }

        public async void JoinParty(UnityEngine.Events.UnityAction callback, string partyCode) 
        {
            var success = await RelayParty.Instance.TryJoinPartyAsync(partyCode);

            if (success)
            {
                Debug.Log($"Sucesso ao entrar na party: {partyCode}");

                callback?.Invoke();
            }
            else
            {
                Debug.Log($"Falha ao entrar na party: {partyCode}");
                _codeInputValidator.ShowCodeInputFieldError();
            }
        }

        public void OnJoinParty() 
        {
            StartCoroutine(Action()); 

            IEnumerator Action()
            {
                yield return new WaitUntil(
                    () => GetComponent<NetworkObject>().IsSpawned 
                    && NetworkManager.Singleton.IsListening); 

                ScreenManager.instance.ToggleScreenByTag(TagManager.CreateRoomMenu.CLIENT_WAITING_ROOM, true);

                var client = NetworkManager.Singleton.LocalClient;
                var playerPref = client.PlayerObject;
                playerPref.gameObject.SetActive(false);

                JoinPartyServerRpc(_nameInputValidator.InputFieldText, NetworkManager.LocalClientId);

                CloseScreen(0.25f);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void JoinPartyServerRpc(string playerName, ulong playerId) 
        {
            var clientDataGO = NetworkSpawner.LocalInstance.InstantiateOnServer("PlayerClientData", Vector3.zero, Quaternion.identity); 
            clientDataGO.name = $"{playerName}PlayerClientData"; 

            var clientData = clientDataGO.GetComponent<PlayerClientData>();

            clientData.SetPlayerData(playerId, playerName);
            ChosenCharacterData playerCharacter = _characterSelectionScreen.GetChosenCharacterData();
            clientData.SetPlayerCharacter(playerCharacter.CharacterName,
                playerCharacter.ChosenCharacterPrefab,
                playerCharacter.CharacterIcon);

            JoinPartyClientRpc();
        }

        [ClientRpc]
        private void JoinPartyClientRpc() 
        {
            Debug.Log("OnJoinPartyClientRPC Called!");
            PlayerClientData playerClientData = FindObjectOfType<PlayerClientData>();

            OnNewPlayerJoined?.Invoke(playerClientData);

            // GameObject playerClientDataGO = NetworkSpawner.LocalInstance.InstantiateOnServer("PlayerClientData", Vector3.zero, Quaternion.identity);
            // playerClientDataGO.name = _nameInputValidator.InputFieldText + "PlayerClientData";
            // PlayerClientData playerClientData = playerClientDataGO.GetComponent<PlayerClientData>();
            // playerClientData.SetPlayerData(NetworkManager.LocalClientId, _nameInputValidator.InputFieldText);

            //CloseScreen(0.25f);
        }
    }
}
