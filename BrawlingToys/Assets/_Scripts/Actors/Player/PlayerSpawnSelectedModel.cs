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
        private void Start()
        {
            SetCharacterModel();
        }

        public void SetCharacterModel()
        {
            StartCoroutine(SetCharacterModelAsync());
        }

        private IEnumerator SetCharacterModelAsync()
        {
            var clientData = GameObject.FindObjectsOfType<PlayerClientData>();
            var currentClient = clientData.First(c => c.PlayerID == NetworkManager.Singleton.LocalClientId); 

            var assetRef = currentClient.SelectedCharacterPrefab;

            if (assetRef != null)
            {
                var handle = assetRef.LoadAssetAsync<GameObject>();

                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var characterModel = handle.Result;
                    var instance = Instantiate(characterModel, transform);
                }
                else
                {
                    Debug.LogError("Falha ao carregar o asset do Addressable.");
                }
                
                Addressables.Release(handle);
            }
            else
            {
                Debug.LogWarning("Referência de asset inválida.");
            }
        }
    }
}