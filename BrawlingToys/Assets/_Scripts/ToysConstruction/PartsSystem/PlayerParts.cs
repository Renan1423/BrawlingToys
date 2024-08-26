using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta classe armazena a posi��o e escala de pe�as atribu�veis a diferentes tamanhos de personagens, usando Transforms e Vector3.
/// A classe PlayerParts permite adicionar novas pe�as ao personagem com m�todos que substituem automaticamente pe�as antigas e usa um dictionary de enum e gameobject para represent�-las.
/// </summary>

public class PlayerParts : MonoBehaviour
{
    [Tooltip("SerializableDictionary que representa posi��o de cada pe�a, especificada por um SkillPartType, no personagem.")]
    [SerializeField]
    private SerializableDictionary<SkinPartType, PlayerPartTransform> _playerPartsPositions;
    private Dictionary<SkinPartType, PlayerPartTransform> _playerPartsPosDict;

    private Dictionary<SkinPartType, GameObject> _equippedParts;

    [SerializeField]
    private Transform partsContainer;

    private void Start()
    {
        _playerPartsPosDict = _playerPartsPositions.ToDictionary();
    }

    public void EquipPart(SkinPartType newPartType, GameObject newPartPrefab) 
    {
        DestroyPart(newPartType);

        Vector3 partLocalPos = _playerPartsPosDict[newPartType].PartLocalPosition;
        Vector3 partLocalScale = _playerPartsPosDict[newPartType].PartLocalScale;

        GameObject newPart = Instantiate(newPartPrefab, partLocalPos, Quaternion.identity, partsContainer);
        newPart.transform.localScale = partLocalScale;

        _equippedParts[newPartType] = newPart;
    }

    public void DestroyPart(SkinPartType partTypeToDestroy) 
    {
        if (_equippedParts[partTypeToDestroy] == null)
            return;

        Destroy(_equippedParts[partTypeToDestroy]);
        _equippedParts[partTypeToDestroy] = null;
    }
}

[System.Serializable]
public struct PlayerPartTransform 
{
    public Vector3 PartLocalPosition;
    public Vector3 PartLocalScale;
}
