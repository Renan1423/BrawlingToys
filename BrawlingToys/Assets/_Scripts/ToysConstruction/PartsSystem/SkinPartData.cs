using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkinPartType 
{ 
    Head,
    Back,
    Face,
    Hand
}

[System.Serializable]
public struct SkinPartData
{
    public SkinPartType SkinPartType;
    [SerializeField]
    private GameObject _partPrefab;

    public GameObject GetPartPrefab() { return _partPrefab; }
}
