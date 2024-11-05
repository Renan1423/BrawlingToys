using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerSpawnSelectedModel : MonoBehaviour {

    [SerializeField] private int _modelNumber;
    [SerializeField] private List<AssetReference> _playerModels;
    AsyncOperationHandle currentPlayerModelOpHandle;

    public event EventHandler modelLoaded;

    private void Start() {
        SetCharacterModel(_modelNumber);
    }

    public void SetCharacterModel(int modelIndex) {
        StartCoroutine(SetCharacterModelAsync(modelIndex));
    }

    IEnumerator SetCharacterModelAsync(int modelIndex) {
        if (currentPlayerModelOpHandle.IsValid())
            Addressables.Release(currentPlayerModelOpHandle);

        var playerModelReference = _playerModels[modelIndex];
        currentPlayerModelOpHandle = playerModelReference.LoadAssetAsync<GameObject>();

        yield return currentPlayerModelOpHandle;

        Instantiate((GameObject)currentPlayerModelOpHandle.Result, gameObject.transform);
        modelLoaded?.Invoke(this, EventArgs.Empty);
    }

}
