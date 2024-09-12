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

        [Header("Enter")]
        [SerializeField] private bool _canChangeGameStateOnEnter;

        [field: SerializeField]
        public GameStateType EnterStateType { get; private set; }

        [Header("Exit")]
        [SerializeField] private bool _canChangeGameStateOnExit;

        [field: SerializeField]
        public GameStateType ExitStateType { get; private set; }



        protected void Start()
        {   
            ScreenManager.instance.OnToggleAnyScreen += ScreenManager_OnToggleAnyScreen;
            gameObject.SetActive(false);
        }

        protected virtual void ScreenManager_OnToggleAnyScreen(object sender, ScreenManager.ToggleAnyScreenEventArgs e) 
        {
            if (e.screenName == ScreenName) 
            {
                Debug.Log("Validou o nome da tela");
                if (e.active && _canChangeGameStateOnEnter)
                {
                    GameManager.LocalInstance.ChangeGameState(EnterStateType);
                }

                if (!e.active && _canChangeGameStateOnExit)
                {
                    GameManager.LocalInstance.ChangeGameState(ExitStateType);
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
