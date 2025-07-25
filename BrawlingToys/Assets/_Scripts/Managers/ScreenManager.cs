using BrawlingToys.DesignPatterns;
using System;
using UnityEngine;

namespace BrawlingToys.Managers
{
    public class ScreenManager : Singleton<ScreenManager>
    {
        public event EventHandler<ToggleAnyScreenEventArgs> OnToggleAnyScreen;

        public class ToggleAnyScreenEventArgs : EventArgs 
        {
            public string screenName;
            public bool active;
        }

        public void ToggleScreenByTag(string tag, bool active) 
        {
            OnToggleAnyScreen?.Invoke(this, new ToggleAnyScreenEventArgs
            {
                screenName = tag,
                active = active
            });
        }
    }
}
