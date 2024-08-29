using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SurpriseBoxSpawner : MonoBehaviour
{
    [SerializeField]
    private AssetLabelReference _buffDebuffAssetLabelReference;
    [SerializeField]
    private SurpriseBoxUI _surpriseBoxUi;
    [SerializeField]
    private GameObject _surpriseBoxPrefab;
    [SerializeField]
    private Transform _surpriseBoxSpawnTrans;

    private void Start()
    {
        SpawnSurpriseBox();
    }

    public void SpawnSurpriseBox() 
    {
        Addressables.LoadAssetsAsync<BuffDebuffTestScriptable>(_buffDebuffAssetLabelReference, null).Completed += SelectBuffDebuff;
    }

    private void SelectBuffDebuff(AsyncOperationHandle<IList<BuffDebuffTestScriptable>> handle) 
    {
        #region If not succeeded
        if (handle.Status != AsyncOperationStatus.Succeeded) 
        {
            Debug.LogError("Failed to load ScriptableObjects from Addressables.");
            return;
        }
        #endregion

        IList<BuffDebuffTestScriptable> foundScriptables = handle.Result;

        #region If no scriptables found
        if (foundScriptables.Count == 0) 
        {
            Debug.LogWarning("No ScriptableObjects of the specified type were found with the given label.");
            return;
        }
        #endregion

        int randomIndex = Random.Range(0, foundScriptables.Count);
        BuffDebuffTestScriptable selectedBuffDebuff = foundScriptables[randomIndex];

        BuildSurpriseBox(selectedBuffDebuff);

    }

    private void BuildSurpriseBox(BuffDebuffTestScriptable buffDebuffInsideBox) 
    {
        GameObject surpriseBoxGo = Instantiate(_surpriseBoxPrefab, Vector3.zero, Quaternion.identity, _surpriseBoxSpawnTrans);
        SurpriseBox surpriseBox = surpriseBoxGo.GetComponent<SurpriseBox>();
        surpriseBox.transform.localPosition = Vector3.zero;

        surpriseBox.SetupSurpriseBox(buffDebuffInsideBox);

        _surpriseBoxUi.SetCurrentSurpriseBox(surpriseBox);
    }
}
