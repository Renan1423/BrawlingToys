using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.Actors
{
    [CreateAssetMenu(fileName = "ModifiersDB", menuName = "Modifier/New Data Base", order = 0)]
    public class ModifiersDB : ScriptableDataBase<ModifierScriptable>
    {
        [Header("Settins")]

        [SerializeField] private bool _acceptInsertions = false; 
        
        [Header("Data Base")]
        
        [SerializeField] private List<ModifierScriptable> _dataBase;


        public override void Add(ModifierScriptable newItem)
        {
            if(!_acceptInsertions) throw new Exception("This data base dont accept insertions");

            _dataBase.Add(newItem); 
        }

        public override void Remove(ModifierScriptable removedItem)
        {
            if(!_acceptInsertions) throw new Exception("This data base dont accept remotions");

            _dataBase.Remove(removedItem); 
        }

        public override ModifierScriptable[] GetCurrentDataBase() => _dataBase.ToArray(); 
    }
}
