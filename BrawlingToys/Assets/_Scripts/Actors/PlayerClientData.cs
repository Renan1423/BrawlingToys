using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.AddressableAssets;

namespace BrawlingToys.Actors
{
    public class PlayerClientData : NetworkBehaviour
    {
        public string PlayerUsername { get; private set; }
        public ulong PlayerID { get; private set; }

        //Selected Character
        public string SelectedCharacterName { get; private set; }
        public AssetReference SelectedCharacterPrefab { get; private set; }
        public Sprite SelectedCharacterSprite { get; private set; }

        //CombatSettings
        public float BuffSpawnChance { get; private set; }
        public float DebuffSpawnChance { get; private set; }
        public int PlayerLife { get; private set; }
        public int RequiredPointsToWin { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void SetPlayerData(ulong playerId, string userName) 
        {
            PlayerID = playerId;
            PlayerUsername = userName;
        }

        public void SetPlayerCharacter(string characterName, AssetReference characterModelPrefab, Sprite characterIcon) 
        {
            SelectedCharacterPrefab = characterModelPrefab;
        }

        public void SetCombatSettings(float buffSpawnChance, float debuffSpawnChance, int playerLife, int requiredPointsToWin) 
        {
            BuffSpawnChance = buffSpawnChance;
            DebuffSpawnChance = debuffSpawnChance;
            PlayerLife = playerLife;
            RequiredPointsToWin = requiredPointsToWin;
        }
    }
}
