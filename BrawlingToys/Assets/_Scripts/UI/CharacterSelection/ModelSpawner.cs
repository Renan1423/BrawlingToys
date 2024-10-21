using BrawlingToys.DesignPatterns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace BrawlingToys.UI
{
    public class ModelSpawner : ContextSingleton<ModelSpawner>
    {
        [Header("Render Texture Camera")]
        [SerializeField]
        private GameObject _renderTextureCamPrefab;
        private Transform _renderTextureCamParentTrans;
        [SerializeField]
        private Vector3 _renderTextureCamParentTransPos;
        private List<RenderTextureCamera> _activeRenderTextureCameras = new();
        //private int _activatedRenderTextureCamsCount => ;

        //Spawns the character model prefab from the asset label reference
        public GameObject SpawnModelFromAssetReference(AssetReference characterAsset, Transform parent)
        {
            GameObject model = Addressables.InstantiateAsync(characterAsset, parent).Result;

            return model;
        }

        #region Spawn Methods
        //Instantiates an object of type RenderTextureCamera in order to render the model on the RawImage from the UI.
        public void SpawnRenderTextureModelWithNewCamera(AssetReference characterAsset, RawImage modelRawImage)
        {
            if (_activeRenderTextureCameras == null)
                _activeRenderTextureCameras = new List<RenderTextureCamera>();

            #region If _renderTextureCamParentTrans is null
            if (_renderTextureCamParentTrans == null)
            {
                GameObject renderTextureCamParentGo = new("RenderTextureCamParent");
                _renderTextureCamParentTrans = renderTextureCamParentGo.transform;
                _renderTextureCamParentTrans.position = _renderTextureCamParentTransPos;
            }
            #endregion

            float posOffSet = -200f * _activeRenderTextureCameras.Count;
            Vector3 renderTextureCamPos = Vector3.one * posOffSet;

            RenderTextureCamera renderTextureCam = SpawnRenderTextureCam(renderTextureCamPos, modelRawImage, characterAsset);

            _activeRenderTextureCameras.Add(renderTextureCam);
        }

        public void ReplaceExistingRenderTextureCamera(int index, AssetReference characterAsset, RawImage modelRawImage)
        {
            if (_activeRenderTextureCameras == null)
                _activeRenderTextureCameras = new List<RenderTextureCamera>();

            #region If _renderTextureCamParentTrans is null
            if (_renderTextureCamParentTrans == null)
            {
                GameObject renderTextureCamParentGo = new("RenderTextureCamParent");
                _renderTextureCamParentTrans = renderTextureCamParentGo.transform;
                _renderTextureCamParentTrans.position = _renderTextureCamParentTransPos;
            }
            #endregion

            if (_activeRenderTextureCameras.Count < index)
            {
                Debug.LogWarning("Index is out of bounds of the _activeRenderTextureCameras list");
                return;
            }

            Destroy(_activeRenderTextureCameras[index].gameObject);

            float posOffSet = -200f * index;
            Vector3 renderTextureCamPos = Vector3.one * posOffSet;

            RenderTextureCamera renderTextureCam = SpawnRenderTextureCam(renderTextureCamPos, modelRawImage, characterAsset);

            _activeRenderTextureCameras[index] = renderTextureCam;
        }

        public RenderTextureCamera SpawnRenderTextureCam(Vector3 renderTextureCamPos, RawImage modelRawImage, AssetReference characterAsset) 
        {
            GameObject renderTextureCamGo = Instantiate(_renderTextureCamPrefab, Vector3.zero, Quaternion.identity, _renderTextureCamParentTrans);
            renderTextureCamGo.transform.localPosition = renderTextureCamPos;
            RenderTextureCamera renderTextureCam = renderTextureCamGo.GetComponent<RenderTextureCamera>();

            modelRawImage.texture = renderTextureCam.SetupRenderTextureCamera();

            GameObject spawnedModel = SpawnModelFromAssetReference(characterAsset, renderTextureCam.GetModelParentTransform());
            renderTextureCam.SetChildModel(spawnedModel);

            return renderTextureCam;
        }

        #endregion

        #region Getters
        public GameObject GetRenderTextureCameraChildModel(int index) 
        {
            return _activeRenderTextureCameras[index].ChildModel;
        }

        public bool HasActiveRenderTextureCamera()
        {
            return _activeRenderTextureCameras.Count > 0;
        }
        #endregion
    }
}