using System.Collections;
using UnityEngine;
using BrawlingToys.Managers;

namespace BrawlingToys.UI
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [field: SerializeField]
        public string ScreenName { get; private set; }

        [Header("Game Manager Itegration")]

        [SerializeField] private bool _canChangeGameState;

        [field: SerializeField]
        public GameStateType StateType { get; private set; }

        

        protected void Start()
        {
            ScreenManager.instance.OnToggleAnyScreen += ScreenManager_OnToggleAnyScreen;
        }

        protected virtual void ScreenManager_OnToggleAnyScreen(object sender, ScreenManager.ToggleAnyScreenEventArgs e) 
        {
            if (e.screenName == ScreenName) 
            {
                if (e.active && _canChangeGameState)
                {
                    GameManager.LocalInstance.ChangeGameState(StateType);
                }

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
