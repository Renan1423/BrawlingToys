using System.Collections;
using UnityEngine;
using BrawlingToys.Managers;
using BrawlingToys.Network;
using BrawlingToys.Core;
using BrawlingToys.Actors;
using Unity.Netcode;
using System;
using UnityEngine.AddressableAssets;
using System.Linq;

namespace BrawlingToys.UI
{
    public class JoinRoom : BaseScreen
    {
        [Header("References")]

        [SerializeField] private GameObject _playerClientDataPrefab; 
        
        [Space]
        
        [SerializeField]
        private CodeInputValidator _codeInputValidator;
        [SerializeField]
        private NameInputValidator _nameInputValidator;

        [SerializeField]
        private CharacterSelectionScreen _characterSelectionScreen;
        [SerializeField]
        private CombatSettingsScreen _combatSettingsScreen;

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
                
                var playerName = _nameInputValidator.InputFieldText; 
                var playerId = NetworkManager.LocalClientId; 
                var characterGUID = _characterSelectionScreen.GetChosenCharacterData().ChosenCharacterPrefab.AssetGUID; 
                
                JoinPartyServerRpc(playerName, playerId, characterGUID);

                CloseScreen(0.25f);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void JoinPartyServerRpc(string playerName, ulong playerId, string characterAssetGUID) 
        {
            MakeClientDataInstance(playerName, playerId, characterAssetGUID); 
            Debug.Log($"server generating: {playerName}");

            var clients = PlayerClientDatasManager.LocalInstance.PlayerClientDatas; 

            var connectedPlayerIds = clients.Select(c => c.PlayerID).ToArray(); 
            var connectedPlayerNames = string.Join(";", clients.Select(c => c.PlayerUsername));
            var connectedPlayerCharacterAssetGUIDs = string.Join(";", clients.Select(c => c.SelectedCharacterPrefab.AssetGUID));
            
            Debug.Log($"Calling Rpc to sync: {PlayerClientDatasManager.LocalInstance.PlayerClientDatas.Count} Players");
            JoinPartyClientRpc(connectedPlayerNames, connectedPlayerIds, connectedPlayerCharacterAssetGUIDs); 
        }

        [ClientRpc]
        private void JoinPartyClientRpc(string playerNamesJoined, ulong[] playerIds, string characterAssetGUIDsJoined) 
        {
            Debug.Log(playerNamesJoined);
            var localData = PlayerClientDatasManager.LocalInstance.PlayerClientDatas; 
            
            var localDataClientsId = localData.Count == 0 
            ? new ulong[0]
            : localData.Select(ld => ld.PlayerID).ToArray(); 

            var playerNames = playerNamesJoined.Split(';');
            var characterAssetGUIDs = characterAssetGUIDsJoined.Split(';');
            
            for (int i = 0; i < playerIds.Length; i++)
            {
                if(!localDataClientsId.Contains(playerIds[i]))
                {
                    MakeClientDataInstance(playerNames[i], playerIds[i], characterAssetGUIDs[i]); 
                }
            }
        }

        private void MakeClientDataInstance(string playerName, ulong playerId, string characterAssetGUID)
        {
            var clientDataGO = Instantiate(_playerClientDataPrefab);

            clientDataGO.name = $"{playerName}PlayerClientData"; 

            var clientData = clientDataGO.GetComponent<PlayerClientData>();

            clientData.SetPlayerData(playerId, playerName);

            var playerCharacter = _characterSelectionScreen.PlayableCharacters.First(pc => pc.CharacterModel.AssetGUID == characterAssetGUID); 
            Debug.Log($"Asset GUID: {playerCharacter.CharacterModel.AssetGUID}");

            clientData.SetPlayerCharacter(playerCharacter.CharacterName,
                playerCharacter.CharacterModel,
                playerCharacter.CharacterIcon);

            var combatSettings = _combatSettingsScreen.GetCombatSettings();

            clientData.SetCombatSettings(combatSettings.BuffSpawnChance, combatSettings.DebuffSpawnChance,
                combatSettings.PlayerLife, combatSettings.RequiredPointsToWin);

            OnNewPlayerJoined?.Invoke(clientData);
        }
    }
}
