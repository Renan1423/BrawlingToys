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
    public class CreateRoomScreen : BaseScreen
    {
        [Space(20)]

        [Header("Create Room Screen")]

        [SerializeField]
        private NameInputValidator _nameInputValidator;
        [SerializeField]
        private GameObject _playerClientDataPrefab;

        public void CreateRoom()
        {
            if (_nameInputValidator.CheckNameValidation())
            {
                CreateParty(OnPartyCreated);
            }
        }

        private void OnPartyCreated(string partyCode) 
        {
            ScreenManager.instance.ToggleScreenByTag(TagManager.CreateRoomMenu.WAITING_FOR_PLAYERS, true);

            //Creating the player client data
            GameObject playerClientDataGO = NetworkSpawner.LocalInstance.InstantiateOnServer("PlayerClientData", Vector3.zero, Quaternion.identity);
            playerClientDataGO.name = _nameInputValidator.InputFieldText + "PlayerClientData";
            PlayerClientData playerClientData = playerClientDataGO.GetComponent<PlayerClientData>();

            playerClientData.SetPlayerData(NetworkManager.LocalClientId, _nameInputValidator.InputFieldText);

            PlayerClientDatasManager.LocalInstance.AddPlayerClientData(playerClientData);

            //Initializing the waiting screen
            WaitingForPlayersScreen waitingForPlayersScreen = FindObjectOfType<WaitingForPlayersScreen>();
            waitingForPlayersScreen.InitializeWaitingRoom(partyCode, _nameInputValidator.InputFieldText);

            CloseScreen(0.25f);
        }

        public async void CreateParty(UnityEngine.Events.UnityAction<string> callback) 
        {
            var operation = await RelayParty.Instance.CreatePartyAsync();

            if (operation.success)
            {
                var textEditor = new TextEditor
                {
                    text = operation.partyCode
                };

                textEditor.SelectAll();
                textEditor.Copy();

                Debug.Log($"Party criada com sucesso, codigo: {operation.partyCode}");

                callback?.Invoke(operation.partyCode);
            }
            else
            {
                Debug.Log("Erro ao criar a party");
            }
        }
    }
}
