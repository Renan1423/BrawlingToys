using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BrawlingToys.UI
{
    public class CharacterModelParent : MonoBehaviour
    {
        [SerializeField]
        private Animator _parentAnim;
        [SerializeField]
        private Animator _containerAnim;

        [SerializeField]
        private Transform _characterModelContainer;

        public void SpawnCharacterModel(AssetReference modelPrefab) 
        {
            ClearCharacterModelContainer();

            SpawnModelFromAssetReference(modelPrefab, _characterModelContainer);
        }

        public GameObject SpawnModelFromAssetReference(AssetReference characterAsset, Transform parent)
        {
            GameObject model = Addressables.InstantiateAsync(characterAsset, parent).Result;

            return model;
        }

        public void ClearCharacterModelContainer() 
        {
            for (int i = 0; i < _characterModelContainer.childCount; i++)
            {
                Destroy(_characterModelContainer.GetChild(i).gameObject);
            }
        }

        public void ShowCharacter() 
        {
            _containerAnim.SetTrigger("Show");
        }

        public void ShowCharacterSelection() 
        {
            _parentAnim.SetTrigger("ShowCharacter");
            _parentAnim.ResetTrigger("ShowSkin");
        }

        public void ShowSkinSelection() 
        {
            _parentAnim.SetTrigger("ShowSkin");
            _parentAnim.ResetTrigger("ShowCharacter");
        }
    }
}
