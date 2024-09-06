using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.Managers
{
    public class ScreenManager : Singleton<ScreenManager>
    {
        public void ToggleScreenByTag(string tag, bool active) 
        {
            GameObject screen = GameObject.FindGameObjectWithTag(tag);

            screen.SetActive(active);
        }
    }
}
