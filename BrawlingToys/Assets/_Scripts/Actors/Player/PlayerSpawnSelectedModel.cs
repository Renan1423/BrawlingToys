using System;
using System.Linq;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class PlayerSpawnSelectedModel : MonoBehaviour
    {
        [SerializeField] private Transform _modelFather; 

        public void SpawnCharacterModel(ulong playerId)
        {
            var allClientData = GameObject.FindObjectsOfType<PlayerClientData>();
            var players = GameObject.FindObjectsOfType<Player>();
            
            var data = allClientData.First(data => data.PlayerID == playerId); 
            var player = players.First(p => p.PlayerId == data.PlayerID);

            var modelSpawner = player.GetComponent<PlayerSpawnSelectedModel>();
            var modelRef = data.SelectedCharacterPrefab;
            
            GameObject playerModel = Instantiate(modelRef, _modelFather); 
        }
    }
}