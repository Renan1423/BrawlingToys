using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.AddressableAssets;
using BrawlingToys.Network;

namespace BrawlingToys.Actors
{
    public class PlayerClientData : NetworkBehaviour
    {
        [field:SerializeField] public string PlayerUsername { get; private set; } 
        [field:SerializeField] public ulong PlayerID { get; private set; } 

        //Selected Character
        [field:SerializeField] public string SelectedCharacterName { get; private set; } 
        [field:SerializeField] public GameObject SelectedCharacterPrefab { get; private set; } 
        [field:SerializeField] public Sprite SelectedCharacterSprite { get; private set; }

        //CombatSettings
        [field:SerializeField] public float BuffSpawnChance { get; private set; }
        [field:SerializeField] public float DebuffSpawnChance { get; private set; }
        [field:SerializeField] public int PlayerLife { get; private set; }
        [field:SerializeField] public int RequiredPointsToWin { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void SetPlayerData(ulong playerId, string userName) 
        {
            PlayerID = playerId;
            PlayerUsername = userName;
        }

        public void SetPlayerCharacter(string characterName, GameObject characterModelPrefab, Sprite characterIcon) 
        {
            SelectedCharacterName = characterName;
            SelectedCharacterPrefab = characterModelPrefab;
            SelectedCharacterSprite = characterIcon;
        }

        public void SetCombatSettings(float buffSpawnChance, float debuffSpawnChance, int playerLife, int requiredPointsToWin) 
        {
            BuffSpawnChance = buffSpawnChance;
            DebuffSpawnChance = debuffSpawnChance;
            PlayerLife = playerLife;
            RequiredPointsToWin = requiredPointsToWin;
        }

        public NetworkSerializedPlayerInfo GetPlayerInfoSerializedData()
        {
            var result = new NetworkSerializedPlayerInfo(
                PlayerUsername,
                PlayerID,
                SelectedCharacterPrefab.name
            ); 

            return result; 
        }

        public NetworkSerializedMatchInfo GetMatchInfoSerializedData()
        {
            var result = new NetworkSerializedMatchInfo(
                BuffSpawnChance,
                DebuffSpawnChance,
                PlayerLife,
                RequiredPointsToWin
            ); 
            
            return result; 
        }
    }
}
