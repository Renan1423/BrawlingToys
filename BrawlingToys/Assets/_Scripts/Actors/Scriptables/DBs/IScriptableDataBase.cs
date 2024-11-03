using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public abstract class ScriptableDataBase<T> : ScriptableObject
    {
        public abstract void Add(T newItem); 
        public abstract void Remove(T removedItem); 

        public abstract T[] GetCurrentDataBase(); 
    }
}
