using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BrawlingToys.Actors;
using BrawlingToys.UI;
using BrawlingToys.Managers;

public class EffectsSelectionScreen : BaseScreen
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
    private Player _player;
    //Variable created for prototype reasons only!
    private Stats _playerStats;

    private void OnEnable()
    {
        GetPlayersInformations();
    }

    public void GetPlayersInformations()
    {
        //We must gather the player informations using the multiplayer features. This is only for prototype purposes
        for (int i = 0; i < 6; i++)
        {
            string playerName = "Player " + i;
            SpawnPlayerInfo(_drawnEffect, _player._stats, playerName, _playerCharacterAssetRef, new GameObject[0]);
        }
    }

    public void SpawnPlayerInfo(ModifierScriptable effectToApply, Stats playerStats, string playerName, AssetReference characterAsset, GameObject[] effectsGo) 
    {
        GameObject playerInfoGo = Instantiate(_playerInfoPrefab, _playerInfoHorizontalLayout);
        PlayerInfoPanel playerInfo = playerInfoGo.GetComponent<PlayerInfoPanel>();

        playerInfo.FillInfoPanel(effectToApply, playerStats, playerName, characterAsset, effectsGo);
        playerInfo.GetPlayerInfoClickEvent().AddListener(OnTargetSelected);
    }

    public void SetDrawnEffect(ModifierScriptable drawnEffect) => _drawnEffect = drawnEffect;

    public void OnTargetSelected(PlayerInfoPanel playerInfoPanel) 
    {
        Debug.Log("Target selected");
        ScreenManager.instance.ToggleScreenByTag(ScreenName, false);
    }
}
