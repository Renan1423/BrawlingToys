using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.UI
{
    public class CreditsScroll : MonoBehaviour
    {
        [SerializeField]
        private CreditsScreen _creditsScreen;

        public void CloseCredits()
        {
            _creditsScreen.CloseCredits();
        }
    }
}
