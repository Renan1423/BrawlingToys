using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffDebuff", menuName = "Prototype/BuffDebuff")]
public class BuffDebuffTestScriptable : ScriptableObject
{
    //public EffectType EffectType;

    [Space(10)]
    [Header("Part")]
    public string PartName;
    public string PartDescription;
    public Sprite Icon;
    public SkinPartType SkinPartType;
    public GameObject SkinPartPrefab;
}
