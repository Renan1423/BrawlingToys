using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
public class ModelSpawner : Singleton<ModelSpawner>
{
    [Header("Render Texture Camera")]
    [SerializeField]
    private GameObject _renderTextureCamPrefab;
    private Transform _renderTextureCamParentTrans;
    [SerializeField]
    private Vector3 _renderTextureCamParentTransPos;
    private int _activatedRenderTextureCamsCount;

    //Spawns the character model prefab from the asset label reference
    public GameObject SpawnModelFromAssetReference(AssetReference characterAsset, Transform parent) 
    {
        GameObject model = Addressables.InstantiateAsync(characterAsset, parent).Result;

        return model;
    }

    //Instantiates an object of type RenderTextureCamera in order to render the model on the RawImage from the UI.
    public void SpawnModelWithRenderTexture(AssetReference characterAsset, RawImage modelRawImage) 
    {
        #region If _renderTextureCamParentTrans is null
        if (_renderTextureCamParentTrans == null) 
        {
            GameObject renderTextureCamParentGo = new("RenderTextureCamParent");
            _renderTextureCamParentTrans = renderTextureCamParentGo.transform;
            _renderTextureCamParentTrans.position = _renderTextureCamParentTransPos;
        }
        #endregion

        float posOffSet = -200f * _activatedRenderTextureCamsCount;
        Vector3 renderTextureCamPos = Vector3.one * posOffSet;

        GameObject renderTextureCamGo = Instantiate(_renderTextureCamPrefab, renderTextureCamPos, Quaternion.identity, _renderTextureCamParentTrans);
        RenderTextureCamera renderTextureCam = renderTextureCamGo.GetComponent<RenderTextureCamera>();

        modelRawImage.texture = renderTextureCam.SetupRenderTextureCamera();

        SpawnModelFromAssetReference(characterAsset, renderTextureCam.GetModelParentTransform());

        _activatedRenderTextureCamsCount++;
    }
}
