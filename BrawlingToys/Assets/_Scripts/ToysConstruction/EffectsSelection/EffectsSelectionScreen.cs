using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BrawlingToys.Actors;

public class EffectsSelectionScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerInfoPrefab;
    [SerializeField]
    private Transform _playerInfoHorizontalLayout;
    [SerializeField]
    //Variable created for prototype reasons only!
    private AssetReference _playerCharacterAssetRef;

    private ModifierScriptable _drawnEffect;
    [SerializeField]
    //Variable created for prototype reasons only!
    private Stats _playerStats;

    private void OnEnable()
    {
        GetPlayersInformations();
    }

    public void GetPlayersInformations()
    {
        for (int i = 0; i < 6; i++)
        {
            string playerName = "Player " + i;
            SpawnPlayerInfo(_drawnEffect, _playerStats, playerName, _playerCharacterAssetRef, new GameObject[0]);
        }
    }

    public void SpawnPlayerInfo(ModifierScriptable effect, Stats playerStats, string playerName, AssetReference characterAsset, GameObject[] effectsGo) 
    {
        GameObject playerInfoGo = Instantiate(_playerInfoPrefab, _playerInfoHorizontalLayout);
        PlayerInfoPanel playerInfo = playerInfoGo.GetComponent<PlayerInfoPanel>();

        playerInfo.FillInfoPanel(effect, playerStats, playerName, characterAsset, effectsGo);
        playerInfo.GetPlayerInfoClickEvent().AddListener(OnTargetSelected);
    }

    public void SetDrawnEffect(ModifierScriptable drawnEffect) => _drawnEffect = drawnEffect;

    public void OnTargetSelected(PlayerInfoPanel playerInfoPanel) 
    {
        this.gameObject.SetActive(false);
    }
}
