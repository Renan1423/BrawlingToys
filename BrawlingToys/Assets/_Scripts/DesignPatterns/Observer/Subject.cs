using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.DesignPatterns
{
    public class Subject : MonoBehaviour
    {
        protected List<Observer> _observers;

        public void RegisterObserver(Observer obs) 
        {
            if (_observers == null)
                _observers = new List<Observer>();

            _observers.Add(obs);
        }

        public void UnregisterObserver(Observer obs) 
        {
            _observers.Remove(obs);
        }

        protected void Notify(string tag) 
        {
            foreach (Observer obs in _observers)
            {
                obs.OnNotify(tag);
            }
        }
    }
}
