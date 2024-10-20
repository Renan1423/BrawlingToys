using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;
using BrawlingToys.Network;
using BrawlingToys.Core;
using BrawlingToys.Actors;
using Unity.Netcode;

namespace BrawlingToys.UI
{
    public class JoinRoom : BaseScreen
    {
        [SerializeField]
        private CodeInputValidator _codeInputValidator;
        [SerializeField]
        private NameInputValidator _nameInputValidator;

        [SerializeField]
        private GameObject _playerClientDataPrefab;

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
            JoinPartyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void JoinPartyServerRpc() 
        {
            GameObject playerClientDataGO = NetworkSpawner.LocalInstance.InstantiateOnServer("PlayerClientData", Vector3.zero, Quaternion.identity);
            PlayerClientData playerClientData = playerClientDataGO.GetComponent<PlayerClientData>();
            playerClientData.SetPlayerData(NetworkManager.LocalClientId, _nameInputValidator.InputFieldText);

            JoinPartyClientRpc();
        }

        [ClientRpc]
        private void JoinPartyClientRpc() 
        {
            ScreenManager.instance.ToggleScreenByTag(TagManager.CreateRoomMenu.CLIENT_WAITING_ROOM, true);

            CloseScreen(0.25f);
        }
    }
}
