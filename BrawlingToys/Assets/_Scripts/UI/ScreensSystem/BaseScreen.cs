using System.Collections;
using UnityEngine;
using BrawlingToys.Managers;
using Unity.Netcode;

namespace BrawlingToys.UI
{
    public abstract class BaseScreen : NetworkBehaviour
    {
        [Header("References")]

        [SerializeField] private GameObject _graphicContainer; 
        
        [field:Header("Tags")]
        
        [field: SerializeField]
        public string ScreenName { get; private set; }

        [Header("Game Manager Integration")]

        [Header("Enter")]
        [SerializeField] private bool _canChangeGameStateOnEnter;

        [field: SerializeField]
        public GameStateType EnterStateType { get; private set; }

        [Header("Exit")]
        [SerializeField] private bool _canChangeGameStateOnExit;

        [field: SerializeField]
        public GameStateType ExitStateType { get; private set; }

        public bool GraphicIsActive { get; private set; }

        protected void Start()
        {   
            ScreenManager.instance.OnToggleAnyScreen += ScreenManager_OnToggleAnyScreen;
            
            _graphicContainer.SetActive(false);
            GraphicIsActive = false; 
        }

        protected virtual void ScreenManager_OnToggleAnyScreen(object sender, ScreenManager.ToggleAnyScreenEventArgs e) 
        {
            if (e.screenName == ScreenName) 
            {
                CheckGameManagerCallbacks(); 

                _graphicContainer.SetActive(e.active);
                GraphicIsActive = e.active; 

                CheckMethodsCallbacks(); 
            }

            void CheckGameManagerCallbacks()
            {
                if (e.active && _canChangeGameStateOnEnter)
                {
                    GameManager.LocalInstance.ChangeGameState(EnterStateType);
                }

                if (!e.active && _canChangeGameStateOnExit)
                {
                    GameManager.LocalInstance.ChangeGameState(ExitStateType);
                }
            }

            void CheckMethodsCallbacks()
            {
                if (e.active)
                {
                    OnScreenEnable(); 
                }
                else
                {
                    OnScreenDisabled(); 
                }
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

        protected virtual void OnScreenEnable()
        {
            return; 
        }

        protected virtual void OnScreenDisabled()
        {
            return; 
        }
    }
}
