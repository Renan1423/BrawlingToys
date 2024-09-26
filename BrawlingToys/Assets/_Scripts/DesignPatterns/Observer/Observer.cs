using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Core;

namespace BrawlingToys.DesignPatterns
{
    public abstract class Observer : MonoBehaviour
    {
        [SerializeField]
        protected Subject _subject;
        [TagSelector, SerializeField]
        protected string _tag;

        protected void OnEnable()
        {
            if (_subject)
            {
                _subject.RegisterObserver(this);
            }
        }

        public abstract void OnNotify(string tag);
    }
}
