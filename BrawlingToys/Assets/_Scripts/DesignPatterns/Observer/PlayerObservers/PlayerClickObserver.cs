using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.DesignPatterns
{
    public class PlayerClickObserver : Observer
    {
        public delegate void OnClickDelegate(bool clickToggle);
        public OnClickDelegate OnClick;

        private bool _clickToggle;

        public override void OnNotify(string tag)
        {
            if (_tag != tag)
                return;

            _clickToggle = !_clickToggle;

            Debug.Log("PlayerClickObserver: Clicked!");

            OnClick?.Invoke(_clickToggle);
        }
    }
}
