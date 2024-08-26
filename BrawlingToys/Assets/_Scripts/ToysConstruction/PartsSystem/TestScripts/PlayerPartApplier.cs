using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartApplier : MonoBehaviour
{
    [SerializeField]
    private SkinPartData _partData;
    [SerializeField]
    private PlayerParts _playerParts;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            ApplyPartToPlayer();
        }
    }

    private void ApplyPartToPlayer() 
    {
        _playerParts.EquipPart(_partData.SkinPartType, _partData.GetPartPrefab());
    }
}
