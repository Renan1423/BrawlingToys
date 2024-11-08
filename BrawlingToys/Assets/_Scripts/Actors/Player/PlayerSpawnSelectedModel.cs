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

        private AsyncOperationHandle<GameObject>? loadedHandle = new();

        public void SetCharacterModel(AssetReference refe)
        {
            StartCoroutine(SetCharacterModelAsync(refe));
        }

        private IEnumerator SetCharacterModelAsync(AssetReference refe)
        {
            var clientData = GameObject.FindObjectsOfType<PlayerClientData>();
            var currentClient = clientData.First(c => c.PlayerID == NetworkManager.Singleton.LocalClientId);

            var assetRef = refe; 

            if (assetRef != null)
            {
                // Log detalhado para verificar o estado do handle antes de tentar carregar o asset
                Debug.Log($"Tentando carregar o asset '{assetRef.RuntimeKey}'.");

                if (loadedHandle.HasValue && loadedHandle.Value.IsValid())
                {
                    Debug.LogWarning($"Asset '{assetRef.RuntimeKey}' já foi carregado e está sendo reutilizado. Evitando carregamento duplicado.");
                }
                else
                {
                    try
                    {
                        // Carrega o asset
                        loadedHandle = assetRef.LoadAssetAsync<GameObject>();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Erro ao carregar o asset '{assetRef.RuntimeKey}': {ex.Message}\nStackTrace: {ex.StackTrace}");
                        yield break;  
                    }
                    
                    Debug.Log($"Carregando o asset '{assetRef.RuntimeKey}'.");

                    yield return loadedHandle.Value;

                    if (loadedHandle.Value.Status == AsyncOperationStatus.Succeeded)
                    {
                        var characterModel = loadedHandle.Value.Result;
                        var instance = Instantiate(characterModel, _modelFather);
                        Debug.Log($"Modelo de personagem '{assetRef.RuntimeKey}' instanciado com sucesso.");
                    }
                    else
                    {
                        Debug.LogError($"Falha ao carregar o asset do Addressable: '{assetRef.RuntimeKey}'.");
                    }
                }

                OnModelSpawed?.Invoke();
            }
            else
            {
                Debug.LogWarning("Referência de asset inválida.");
            }
        }
    }
}