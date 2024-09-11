using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;

namespace BrawlingToys.UI
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [field: SerializeField]
        public string ScreenName { get; private set; }

        protected void Start()
        {
            ScreenManager.instance.OnToggleAnyScreen += ScreenManager_OnToggleAnyScreen;
        }

        protected virtual void ScreenManager_OnToggleAnyScreen(object sender, ScreenManager.ToggleAnyScreenEventArgs e) 
        {
            if (e.screenName == ScreenName) 
            {
                this.gameObject.SetActive(e.active);
            }
        }

        protected virtual void CloseScreen(float delayToClose) 
        {
            StartCoroutine(CloseScreenCoroutine(delayToClose));
        }

        private IEnumerator CloseScreenCoroutine(float delayToClose) 
        {
            yield return new WaitForSeconds(delayToClose);

            gameObject.SetActive(false);
        }
    }
}
