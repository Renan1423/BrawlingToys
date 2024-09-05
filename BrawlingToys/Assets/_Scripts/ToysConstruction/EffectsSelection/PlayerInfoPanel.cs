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
    private ModifierScriptable _effect;
    private Stats _playerStats;
    [SerializeField]
    private TextMeshProUGUI _playerName;
    [SerializeField]
    private RawImage _modelRawImage;
    [SerializeField]
    private Transform _effectsHorizontalLayout;
    [SerializeField]
    private UnityEvent<PlayerInfoPanel> _onClicked;

    public void FillInfoPanel(ModifierScriptable effect, Stats playerStats, string playerName, AssetReference characterAsset, GameObject[] effectsGo) 
    {
        _effect = effect;
        _playerStats = playerStats;
        _playerName.text = playerName;

        foreach (GameObject go in effectsGo)
        {
            go.transform.SetParent(_effectsHorizontalLayout);
        }

        ModelSpawner.instance.SpawnModelWithRenderTexture(characterAsset, _modelRawImage);
    }

    public UnityEvent<PlayerInfoPanel> GetPlayerInfoClickEvent() => _onClicked;

    public void OnPlayerInfoPanelClicked() 
    {
        _onClicked?.Invoke(this);

        _playerStats.Mediator.AddModifier(_effect.CreateModifier());
    }
}
