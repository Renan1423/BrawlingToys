using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Actors;

public class EffectIconGenerator : Singleton<EffectIconGenerator>
{
    [SerializeField]
    private GameObject _effectIconPrefab;

    public void CreateEffectIcon(ModifierScriptable effect, Transform iconParent)
    {
        GameObject effectIconGo = Instantiate(_effectIconPrefab, iconParent);
        EffectIcon effectIcon = effectIconGo.GetComponent<EffectIcon>();

        effectIcon.SetupIcon(effect.EffectName, effect.EffectDescription, effect.EffectIcon);
    }
}
