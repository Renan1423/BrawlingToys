using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.DesignPatterns;

namespace BrawlingToys.Actors
{
    /// <summary>
    /// Esta classe armazena a posição e escala de peças atribuíveis a diferentes tamanhos de personagens, usando Transforms e Vector3.
    /// A classe PlayerParts permite adicionar novas peças ao personagem com métodos que substituem automaticamente peças antigas e usa um dictionary de enum e gameobject para representá-las.
    /// </summary>

    public class PlayerParts : MonoBehaviour
    {
        [Tooltip("SerializableDictionary que representa posição de cada peça, especificada por um SkillPartType, no personagem.")]
        [SerializeField]
        private SerializableDictionary<SkinPartType, PlayerPartTransform> _playerPartsPositions;
        private Dictionary<SkinPartType, PlayerPartTransform> _playerPartsPosDict;

        private GameObject _equippedPart;

        #region In case we want to equip different parts in the same character

        //private Dictionary<SkinPartType, GameObject> _equippedParts;

        /*private void CreateEquippedPartsDictionary() 
        {
            _equippedParts = new Dictionary<SkinPartType, GameObject>();

            for (int i = 0; i < System.Enum.GetNames(typeof(SkinPartType)).Length; i++)
            {
                _equippedParts.Add((SkinPartType) i, null);
            }
        }

        public void EquipPart(SkinPartType newPartType, GameObject newPartPrefab) 
        {
            DestroyPart(newPartType);

            Transform partTrans = _playerPartsPosDict[newPartType].PartPosition;
            Vector3 partLocalScale = _playerPartsPosDict[newPartType].PartLocalScale;

            GameObject newPart = Instantiate(newPartPrefab, partTrans.position, Quaternion.identity, partTrans);
            newPart.transform.localScale = partLocalScale;

            if (_equippedParts == null)
                CreateEquippedPartsDictionary();

            _equippedParts[newPartType] = newPart;
        }

        public void DestroyPart(SkinPartType partTypeToDestroy) 
        {
            if (_equippedParts == null)
                return;

            if (_equippedParts[partTypeToDestroy] == null)
                return;

            Destroy(_equippedParts[partTypeToDestroy]);
            _equippedParts[partTypeToDestroy] = null;
        }

         */

        #endregion

        private void Start()
        {
            _playerPartsPosDict = _playerPartsPositions.ToDictionary();
        }

        public void EquipPart(SkinPartType newPartType, GameObject newPartPrefab)
        {
            DestroyPart();

            Transform partTrans = _playerPartsPosDict[newPartType].PartPosition;
            Vector3 partLocalScale = _playerPartsPosDict[newPartType].PartLocalScale;

            GameObject newPart = Instantiate(newPartPrefab, partTrans.position, Quaternion.identity, partTrans);
            newPart.transform.localScale = partLocalScale;

            _equippedPart = newPart;
        }

        public void DestroyPart()
        {
            if (_equippedPart == null)
                return;

            Destroy(_equippedPart);
            _equippedPart = null;
        }
    }

    [System.Serializable]
    public struct PlayerPartTransform
    {
        public Transform PartPosition;
        public Vector3 PartLocalScale;
    }
}