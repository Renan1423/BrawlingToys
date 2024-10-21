using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace BrawlingToys.Actors
{
    public class PlayerClientData : NetworkBehaviour
    {
        public string PlayerUsername { get; private set; }
        public ulong PlayerID { get; private set; }
        public GameObject SelectedCharacterPrefab { get; private set; }

        public void SetPlayerData(ulong playerId, string userName) 
        {
            PlayerID = playerId;
            PlayerUsername = userName;
        }

        public void SetPlayerCharacter(GameObject selectedCharacterPrefab) 
        {
            SelectedCharacterPrefab = selectedCharacterPrefab;
        }
    }
}
