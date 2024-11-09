using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;
using BrawlingToys.Network;
using BrawlingToys.Core;
using BrawlingToys.Actors;
using Unity.Netcode;
using UnityEngine.UI;
using System.Linq;

namespace BrawlingToys.UI
{
    public class CreateRoomScreen : BaseScreen
    {
        [Header("References")]

        [SerializeField] private GameObject _playerClientData; 
        
        [Space(20)]

        [Header("Create Room Screen")]

        [SerializeField]
        private NameInputValidator _nameInputValidator;
        [SerializeField]
        private CharacterSelectionScreen _characterSelectionScreen;
        [SerializeField]
        private CombatSettingsScreen _combatSettingsScreen;
        [SerializeField]
        private Button _createPartyButton;
        [SerializeField]
        private Button _settingsButton;
        
        protected override void OnScreenEnable()
        {
            ToggleButtons(true);
        }

        public void CreateRoom()
        {
            if (_nameInputValidator.CheckNameValidation())
            {
                //ToggleButtons(false);
                CreateParty(OnPartyCreatedCallback);
            }
        }

        private void OnPartyCreatedCallback(string partyCode) 
        {
            ScreenManager.instance.ToggleScreenByTag(TagManager.CreateRoomMenu.WAITING_FOR_PLAYERS, true);

            var playerName = _nameInputValidator.InputFieldText; 
            var playerId = NetworkManager.LocalClientId; 
            var characterGUID = _characterSelectionScreen.GetChosenCharacterData().ChosenCharacterPrefab.AssetGUID;
            
            JoinPartyServerRpc(playerName, playerId, characterGUID); 

            WaitingForPlayersScreen waitingForPlayersScreen = FindObjectOfType<WaitingForPlayersScreen>();
            waitingForPlayersScreen.InitializeWaitingRoom(partyCode, _nameInputValidator.InputFieldText);

            CloseScreen(0.25f);
        }

        [ServerRpc]
        private void JoinPartyServerRpc(string playerName, ulong playerId, string characterAssetGUID)
        {
            var clientDataGO = Instantiate(_playerClientData);

            clientDataGO.name = $"{playerName}PlayerClientData"; 

            var clientData = clientDataGO.GetComponent<PlayerClientData>();

            clientData.SetPlayerData(playerId, playerName);

            var playerCharacter = _characterSelectionScreen.PlayableCharacters.First(pc => pc.CharacterModel.AssetGUID == characterAssetGUID); 
            Debug.Log($"Asset GUID: {playerCharacter.CharacterModel.AssetGUID}");

            clientData.SetPlayerCharacter(playerCharacter.CharacterName,
                playerCharacter.CharacterModel,
                playerCharacter.CharacterIcon);

            PlayerClientDatasManager.LocalInstance.AddPlayerClientData(clientData);
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
                ToggleButtons(true);
                Debug.Log("Erro ao criar a party");
            }
        }

        public void OpenCombatSettingsScreen() 
        {
            ScreenManager.instance.ToggleScreenByTag(TagManager.CreateRoomMenu.COMBAT_SETTINGS, true);

            CloseScreen(0.25f);
        }

        private void ToggleButtons(bool result) 
        {
            _createPartyButton.interactable = result;
            _settingsButton.interactable = result;
        }
    }
}
