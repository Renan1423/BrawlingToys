using System;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BrawlingToys.Actors
{
    public class PlayerSpawnSelectedModel : MonoBehaviour
    {
        [SerializeField] private Transform _modelFather; 
        
        public Action OnModelSpawed;

        public void SetCharacterModel(GameObject refe)
        {
            GameObject playerModel = Instantiate(refe, _modelFather);
        }
    }
}