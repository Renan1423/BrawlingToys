using System.Collections;
using UnityEngine;
using BrawlingToys.Managers;
using BrawlingToys.Network;
using BrawlingToys.Core;
using BrawlingToys.Actors;
using Unity.Netcode;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

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
                
                var playerInfo = GenerateSerializedPlayerInfo(); 
                var playerInfoJSON = JsonUtility.ToJson(playerInfo); 
                
                RegisterClientServerRpc(playerInfoJSON);

                CloseScreen(0.25f);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RegisterClientServerRpc(string playerInfoJSON) 
        {
            var playerInfo = JsonUtility
                .FromJson<NetworkSerializedPlayerInfo>(playerInfoJSON); 
            
            MakeClientDataInstance(playerInfo); 

            var clients = PlayerClientDatasManager.LocalInstance.PlayerClientDatas;

            //var clientPlayerDataSerialized = clients    
            //    .Select(client => client.GetPlayerInfoSerializedData())
            //    .ToArray(); 

            var clientPlayerDataSerialized = new List<NetworkSerializedPlayerInfo>();

            foreach (var client in clients)
            {
                clientPlayerDataSerialized.Add(client.GetPlayerInfoSerializedData()); 
            }

            var playerInfosJSON = JsonConvert.SerializeObject(clientPlayerDataSerialized);

            Debug.Log("pass"); 
            Debug.Log(playerInfosJSON); 
            
            var matchInfo = clients
                .First(c => c.PlayerID == 0)
                .GetMatchInfoSerializedData(); 

            var matchInfoJSON = JsonConvert.SerializeObject(matchInfo); 
            
            RegisterClientClientRpc(playerInfoJSON, matchInfoJSON); 
        }

        [ClientRpc]
        private void RegisterClientClientRpc(string playerInfosJSON, string matchInfoJSON) 
        {
            var playerInfos = JsonConvert
                .DeserializeObject<NetworkSerializedPlayerInfo[]>(playerInfosJSON);

            var matchInfo = JsonConvert
                .DeserializeObject<NetworkSerializedMatchInfo>(matchInfoJSON); 
            
            var localData = PlayerClientDatasManager.LocalInstance.PlayerClientDatas; 
            var localDataClientsId = localData
                .Select(data => data.PlayerID)
                .ToArray(); 
            
            for (int i = 0; i < playerInfos.Length; i++)
            {
                var currentData = playerInfos[i]; 
                
                if(!localDataClientsId.Contains(currentData.PlayerId))
                {
                    if(IsHostPlayer(currentData.PlayerId))
                    {
                        MakeHostDataInstance(currentData, matchInfo); 
                    }   
                    else
                    {
                        MakeClientDataInstance(currentData); 
                    }
                }
            }

            bool IsHostPlayer(ulong playerId) => playerId == 0; 
        }

        private void MakeClientDataInstance(NetworkSerializedPlayerInfo playerInfo)
        {
            var clientDataGO = Instantiate(_playerClientDataPrefab);

            clientDataGO.name = $"{playerInfo.PlayerName}PlayerClientData"; 

            var clientData = clientDataGO.GetComponent<PlayerClientData>();

            clientData.SetPlayerData(playerInfo.PlayerId, playerInfo.PlayerName);

            var playerCharacter = _characterSelectionScreen.PlayableCharacters
                .First(pc => pc.CharacterModel.AssetGUID == playerInfo.CharacterAssetGUID); 

            clientData.SetPlayerCharacter(playerCharacter.CharacterName,
                playerCharacter.CharacterModel,
                playerCharacter.CharacterIcon);

            OnNewPlayerJoined?.Invoke(clientData);
        }

        private void MakeHostDataInstance(NetworkSerializedPlayerInfo playerInfo, NetworkSerializedMatchInfo matchInfo)
        {
            var clientDataGO = Instantiate(_playerClientDataPrefab);

            clientDataGO.name = $"{playerInfo.PlayerName}PlayerClientData"; 

            var clientData = clientDataGO.GetComponent<PlayerClientData>();

            clientData.SetPlayerData(playerInfo.PlayerId, playerInfo.PlayerName);

            var playerCharacter = _characterSelectionScreen.PlayableCharacters
                .First(pc => pc.CharacterModel.AssetGUID == playerInfo.CharacterAssetGUID); 

            clientData.SetPlayerCharacter(playerCharacter.CharacterName,
                playerCharacter.CharacterModel,
                playerCharacter.CharacterIcon);

            clientData.SetCombatSettings(
                matchInfo.BuffSpawnChance,
                matchInfo.DebuffSpawnChance,
                matchInfo.PlayerLife,
                matchInfo.RequiredPointsToWin
            ); 

            OnNewPlayerJoined?.Invoke(clientData);
        }

        private NetworkSerializedPlayerInfo GenerateSerializedPlayerInfo()
        {
            var playerName = _nameInputValidator.InputFieldText; 
            var playerId = NetworkManager.LocalClientId; 
            var characterGUID = _characterSelectionScreen.GetChosenCharacterData().ChosenCharacterPrefab.AssetGUID; 

            var info = new NetworkSerializedPlayerInfo(
                playerName,
                playerId,
                characterGUID
            ); 

            return info; 
        }
    }
}
