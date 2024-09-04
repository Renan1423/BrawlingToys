using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectIconGenerator : Singleton<EffectIconGenerator>
{
    [SerializeField]
    private GameObject _effectIconPrefab;

    public void CreateEffectIcon(BuffDebuffTestScriptable effect, Transform iconParent)
    {
        GameObject effectIconGo = Instantiate(_effectIconPrefab, iconParent);
        EffectIcon effectIcon = effectIconGo.GetComponent<EffectIcon>();

        effectIcon.SetupIcon(effect.PartName, effect.PartDescription, effect.Icon);
    }
}
