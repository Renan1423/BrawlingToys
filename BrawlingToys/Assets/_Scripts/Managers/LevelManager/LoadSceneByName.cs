using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.Managers
{
    public class LoadSceneByName : MonoBehaviour
    {
        public void LaodScene(string scene)
        {
            LevelManager.LocalInstance.LocalLoadSceneByName(scene); 
        }
    }
}
