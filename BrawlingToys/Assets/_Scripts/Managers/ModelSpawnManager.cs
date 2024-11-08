using BrawlingToys.Actors;
using BrawlingToys.DesignPatterns;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Managers
{
    public class ModelSpawnManager : ContextSingleton<ModelSpawnManager>
    {
        public void InstantietePlayersModels()
        {
            var clientData = GameObject.FindObjectsOfType<PlayerClientData>();
            var players = GameObject.FindObjectsOfType<Player>();

            foreach (var data in clientData)
            {
                var player = players.First(p => p.PlayerId == data.PlayerID);

                var modelSpawner = player.GetComponent<PlayerSpawnSelectedModel>();
                var modelRef = data.SelectedCharacterPrefab;

                modelSpawner.SetCharacterModel(modelRef);
            }
        }
    }
}
