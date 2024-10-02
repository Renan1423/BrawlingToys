using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BrawlingToys.Actors;
using BrawlingToys.UI;
using BrawlingToys.Managers;
using Unity.Netcode;
using System.Linq;
using UnityEngine.InputSystem;

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
    private List<Player> _players;
    //Variable created for prototype reasons only!
    private Stats _playerStats;

    [Header("Inputs")]

    [SerializeField] private InputActionAsset _inputActionAsset; 
    private InputActionMap _gameplayMap; 

    private bool _initialized; 

    protected override void OnScreenEnable()
    {
        if(!_initialized)
        {
            _players = MatchManager.LocalInstance.MatchPlayers.ToList(); 
            DrawScreen();  

            _initialized = true; 
        }
    }


    public void DrawScreen()
    {
        //We must gather the player informations using the multiplayer features. This is only for prototype purpose
        for (int i = 0; i < _players.Count; i++)
        {
            string playerName = "Player " + i;
            SpawnPlayerInfo(_drawnEffect, _players[i].Stats, playerName, _playerCharacterAssetRef, new GameObject[0]);
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
