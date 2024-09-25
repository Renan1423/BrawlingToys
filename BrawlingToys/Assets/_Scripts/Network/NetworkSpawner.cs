using System;
using System.Collections.Generic;
using BrawlingToys.Network;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace BrawlingToys.Network
{
    /// <summary>
    /// This class manage the game object instantiation on server, this take care of all network responsibilities to do that on every client  
    /// </summary>
    public class NetworkSpawner : NetworkSingleton<NetworkSpawner> 
    {
        /// <summary>
        /// Facilitates the search for network prefabs, associates the prefab with its name as a key
        /// </summary>
        private Dictionary<string, GameObject> _networkPrefabs = new(); 

        /// <summary>
        /// Called when any game object spawn on server, and return it
        /// </summary>
        [HideInInspector] public UnityEvent<string, GameObject> WhenObjectSpawnedOnServer = new(); 

        protected override void Awake()
        {
            base.Awake();

            GenerateNetworkDictionary();
        }

        private void Start()
        {
            Debug.Log(GetComponent<NetworkObject>().NetworkManager);
        }

        private void GenerateNetworkDictionary()
        {
            var networkPrefabLists = NetworkManager.Singleton.NetworkConfig.Prefabs.NetworkPrefabsLists;

            foreach (var prefabList in networkPrefabLists)
            {
                foreach (var prefab in prefabList.PrefabList)
                {
                    Debug.Log(prefab.Prefab.name, prefab.Prefab);
                    _networkPrefabs.Add(prefab.Prefab.name, prefab.Prefab); 
                }
            }
        }

        /// <summary>
        /// Instantiate the object on server, for every connected client.
        /// </summary>
        /// <param name="gameObjectName">The name of the object to spawn.</param>
        /// <param name="position">The position to spawn the object at.</param>
        /// <param name="rotation">The rotation to spawn the object with.</param>
        public GameObject InstantiateOnServer(string gameObjectName, Vector3 position, Quaternion rotation)
        {
            if (!NameExistsInNetworkPrefabs())
            {
                MakeDictionaryLogError();
                return null;  
            }

            if (!NetworkManager.Singleton.IsServer)
            {
                MakeServerLogError();
                return null; 
            }

            var response = GetNetworkSpawnerObject(); 
            return response;

            GameObject GetNetworkSpawnerObject()
            {
                var prefab = _networkPrefabs[gameObjectName]; 
            
                var objectToSpawn = Instantiate(prefab, position, rotation);
                objectToSpawn.GetComponent<NetworkObject>().Spawn(true);

                return objectToSpawn; 
            }

            bool NameExistsInNetworkPrefabs() => _networkPrefabs.ContainsKey(gameObjectName); 
            
            void MakeDictionaryLogError() => 
            Debug.LogError($"The object name {gameObjectName} dont match with network prefabs list names, please check network prefab names");
            void MakeServerLogError() => 
            Debug.LogError($"This call can be executed in server only!");
        }
    }
}
