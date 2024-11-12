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

            var playerInfo = new NetworkSerializedPlayerInfo(
                playerName,
                playerId,
                characterGUID
            ); 

            var playerInfoJSON = JsonUtility.ToJson(playerInfo); 

            var uiMatchData = _combatSettingsScreen.GetCombatSettings(); 
            var matchInfo = new NetworkSerializedMatchInfo(
                uiMatchData.BuffSpawnChance,
                uiMatchData.DebuffSpawnChance,
                uiMatchData.PlayerLife,
                uiMatchData.RequiredPointsToWin
            );  

            var matchInfoJSON = JsonUtility.ToJson(matchInfo); 
            
            JoinPartyServerRpc(playerInfoJSON, matchInfoJSON); 

            WaitingForPlayersScreen waitingForPlayersScreen = FindObjectOfType<WaitingForPlayersScreen>();
            waitingForPlayersScreen.InitializeWaitingRoom(partyCode, _nameInputValidator.InputFieldText);

            CloseScreen(0.25f);
        }

        [ServerRpc]
        private void JoinPartyServerRpc(string playerInfoJSON, string matchInfoJSON)
        {
            var playerInfo = JsonUtility.FromJson<NetworkSerializedPlayerInfo>(playerInfoJSON); 
            var matchInfo = JsonUtility.FromJson<NetworkSerializedMatchInfo>(matchInfoJSON); 
            
            var clientDataGO = Instantiate(_playerClientData);

            clientDataGO.name = $"{playerInfo.PlayerName}PlayerClientData"; 

            var clientData = clientDataGO.GetComponent<PlayerClientData>();

            clientData.SetPlayerData(playerInfo.PlayerId, playerInfo.PlayerName);

            var playerCharacter = _characterSelectionScreen.PlayableCharacters.First(pc => pc.CharacterModel.AssetGUID == playerInfo.CharacterAssetGUID); 

            clientData.SetPlayerCharacter(playerCharacter.CharacterName,
                playerCharacter.CharacterModel,
                playerCharacter.CharacterIcon);
            
            clientData.SetCombatSettings(
                buffSpawnChance: matchInfo.BuffSpawnChance,
                debuffSpawnChance: matchInfo.DebuffSpawnChance,
                playerLife: matchInfo.PlayerLife,
                requiredPointsToWin: matchInfo.RequiredPointsToWin
            ); 

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
