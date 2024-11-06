using BrawlingToys.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BrawlingToys.Actors
{
    public class PlayerSpawnSelectedModel : MonoBehaviour
    {
        [SerializeField] private int _modelNumber;
        [SerializeField] private List<AssetReference> _playerModels;
        AsyncOperationHandle currentPlayerModelOpHandle;

        public event Action OnModelLoaded;

        private void Start()
        {
            SetCharacterModel(_modelNumber);
        }

        public void SetCharacterModel(int modelIndex)
        {
            StartCoroutine(SetCharacterModelAsync(modelIndex));
        }

        IEnumerator SetCharacterModelAsync(int modelIndex)
        {
            if (currentPlayerModelOpHandle.IsValid())
                Addressables.Release(currentPlayerModelOpHandle);

            var playerModelReference = _playerModels[modelIndex];
            currentPlayerModelOpHandle = playerModelReference.LoadAssetAsync<GameObject>();

            yield return currentPlayerModelOpHandle;

            Instantiate((GameObject)currentPlayerModelOpHandle.Result, gameObject.transform);
            OnModelLoaded?.Invoke();
        }


        //public Action OnModelInstantiate;

        //[SerializeField] private Transform _playerModelParent;

        //private void Start()
        //{
        //    SetCharacterModel();
        //}

        //public void SetCharacterModel()
        //{
        //    var clientDatas = GameObject.FindObjectsOfType<PlayerClientData>();
        //    var client = clientDatas.First(cd => cd.PlayerID == NetworkManager.Singleton.LocalClientId);

        //    AssetReference refereceModel = client.SelectedCharacterPrefab;
        //    AsyncOperationHandle<GameObject> selectedModel = refereceModel.LoadAssetAsync<GameObject>(); ;
        //    selectedModel.Completed += HandleModelLoaded;

        //    Addressables.Release(selectedModel);

        //}

        //private void HandleModelLoaded(AsyncOperationHandle<GameObject> model)
        //{
        //    if (model.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        Debug.Log("Modelo Instanciado");
        //        Instantiate(model.Result, _playerModelParent);
        //        OnModelInstantiate?.Invoke();
        //    }
        //    else
        //    {
        //        Debug.LogError("PlayerSpawnSelectedModel: Erro ao carregar modelo de forma assicrona");
        //    }
        //}
    }
}