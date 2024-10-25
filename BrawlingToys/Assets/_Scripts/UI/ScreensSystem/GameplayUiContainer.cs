using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.UI
{
    public class GameplayUiContainer : MonoBehaviour
    {
        //This class is a singleton, however there's no reason to don't destroy it on load
        public static GameplayUiContainer instance;

        [field: SerializeField]
        public QuestionScreen QuestionScreen { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}
