using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using BrawlingToys.Actors;

namespace BrawlingToys.UI
{
    public class SurpriseBoxSpawner : MonoBehaviour
    {
        [SerializeField]
        private AssetLabelReference _buffDebuffAssetLabelReference;
        [SerializeField]
        private SurpriseBoxUI _surpriseBoxUi;
        [SerializeField]
        private GameObject _surpriseBoxPrefab;
        private Transform _surpriseBoxSpawnTrans;
        [SerializeField]
        private GameObject _surpriseBoxEnvironment;

        private void OnEnable()
        {
            SpawnSurpriseBoxEnvironment();
            SpawnSurpriseBox();
        }

        private void OnDisable()
        {
            Destroy(_surpriseBoxSpawnTrans.parent.gameObject);
            _surpriseBoxSpawnTrans = null;
        }

        private void SpawnSurpriseBoxEnvironment()
        {
            GameObject surpriseBoxEnvironmentGo = Instantiate(_surpriseBoxEnvironment);
            SurpriseBoxEnvironment surpriseBoxEnvironment = surpriseBoxEnvironmentGo.GetComponent<SurpriseBoxEnvironment>();

            _surpriseBoxSpawnTrans = surpriseBoxEnvironment.SurpriseBoxSpawnTrans;
        }

        public void SpawnSurpriseBox()
        {
            Addressables.LoadAssetsAsync<ModifierScriptable>(_buffDebuffAssetLabelReference, null).Completed += SelectBuffDebuff;
        }

        private void SelectBuffDebuff(AsyncOperationHandle<IList<ModifierScriptable>> handle)
        {
            #region If not succeeded
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("SurpriseBoxSpawner: Failed to load ScriptableObjects from Addressables.");
                return;
            }
            #endregion

            IList<ModifierScriptable> foundScriptables = handle.Result;

            #region If no scriptables found
            if (foundScriptables.Count == 0)
            {
                Debug.LogWarning("SurpriseBoxSpawner: No ScriptableObjects of the specified type were found with the given label.");
                return;
            }
            #endregion

            int randomIndex = Random.Range(0, foundScriptables.Count);
            ModifierScriptable selectedBuffDebuff = foundScriptables[randomIndex];

            BuildSurpriseBox(selectedBuffDebuff);

        }

        private void BuildSurpriseBox(ModifierScriptable buffDebuffInsideBox)
        {
            GameObject surpriseBoxGo = Instantiate(_surpriseBoxPrefab, Vector3.zero, Quaternion.identity, _surpriseBoxSpawnTrans);
            SurpriseBox surpriseBox = surpriseBoxGo.GetComponent<SurpriseBox>();
            surpriseBox.transform.localPosition = Vector3.zero;

            surpriseBox.SetupSurpriseBox(buffDebuffInsideBox);

            _surpriseBoxUi.SetCurrentSurpriseBox(surpriseBox);
            _surpriseBoxUi.EnableOpenButton();
        }
    }
}