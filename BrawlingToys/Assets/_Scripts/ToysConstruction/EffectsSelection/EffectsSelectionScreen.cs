using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EffectsSelectionScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerInfoPrefab;
    [SerializeField]
    private Transform _playerInfoHorizontalLayout;
    [SerializeField]
    //Variable created for prototype reasons only!
    private AssetReference _playerCharacterAssetRef;

    private BuffDebuffTestScriptable _drawnEffect;

    private void OnEnable()
    {
        GetPlayersInformations();
    }

    public void GetPlayersInformations()
    {
        for (int i = 0; i < 6; i++)
        {
            string playerName = "Player " + i;
            SpawnPlayerInfo(playerName, _playerCharacterAssetRef, new GameObject[0]);
        }
    }

    public void SpawnPlayerInfo(string playerName, AssetReference characterAsset, GameObject[] effectsGo) 
    {
        GameObject playerInfoGo = Instantiate(_playerInfoPrefab, _playerInfoHorizontalLayout);
        PlayerInfoPanel playerInfo = playerInfoGo.GetComponent<PlayerInfoPanel>();

        playerInfo.FillInfoPanel(playerName, characterAsset, effectsGo);
        playerInfo.GetPlayerInfoClickEvent().AddListener(OnTargetSelected);
    }

    public void SetDrawnEffect(BuffDebuffTestScriptable drawnEffect) => _drawnEffect = drawnEffect;

    public void OnTargetSelected(PlayerInfoPanel playerInfoPanel) 
    {
        this.gameObject.SetActive(false);
    }
}
