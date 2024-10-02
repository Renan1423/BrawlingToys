using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using BrawlingToys.Actors;
using TMPro;

public class PlayerInfoPanel : MonoBehaviour
{
    private ModifierScriptable _effectToApply;
    private Stats _playerStats;
    [SerializeField]
    private TextMeshProUGUI _playerName;
    [SerializeField]
    private RawImage _modelRawImage;
    [SerializeField]
    private Transform _effectsHorizontalLayout;
    [SerializeField]
    private UnityEvent<PlayerInfoPanel> _onClicked;

    public void FillInfoPanel(ModifierScriptable effectToApply, Stats playerStats, string playerName, AssetReference characterAsset, GameObject[] effectsGo) 
    {
        _effectToApply = effectToApply;
        _playerStats = playerStats;
        _playerName.text = playerName;

        foreach (GameObject go in effectsGo)
        {
            go.transform.SetParent(_effectsHorizontalLayout);
        }

        CreateEffectIcons(playerStats);

        ModelSpawner.Instance.SpawnModelWithRenderTexture(characterAsset, _modelRawImage);
    }

    private void CreateEffectIcons(Stats playerStats)
    {
        if (playerStats.Mediator.GetAppliedModifiers() == null)
            return;

        foreach (ModifierScriptable mod in playerStats.Mediator.GetAppliedModifiers())
        {
            EffectIconGenerator.instance.CreateEffectIcon(mod, _effectsHorizontalLayout);
        }
    }

    public UnityEvent<PlayerInfoPanel> GetPlayerInfoClickEvent() => _onClicked;

    public void OnPlayerInfoPanelClicked() 
    {
        _onClicked?.Invoke(this);

        _playerStats.Mediator.AddModifier(_effectToApply);
    }
}
