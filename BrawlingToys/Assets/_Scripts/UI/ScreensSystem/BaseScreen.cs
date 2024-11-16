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

        [Header("Previous Screen")]
        [SerializeField]
        private string _previousScreenName;
        [SerializeField]
        private float _previousScreenTransitionDelay = 0.25f;

        [Header("Auto Activate")]
        [SerializeField]
        private bool _autoActivate;

        [Header("Animation")]
        [SerializeField]
        private bool _performAnimationOnClose;
        [SerializeField]
        private Animator _anim;
        [SerializeField]
        private string _closeTrigger = "Close";

        protected virtual void Start()
        {   
            ScreenManager.instance.OnToggleAnyScreen += ScreenManager_OnToggleAnyScreen;
            
            ToggleGraphicContainer(_autoActivate);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            ScreenManager.instance.OnToggleAnyScreen -= ScreenManager_OnToggleAnyScreen;
        }



        protected virtual void ScreenManager_OnToggleAnyScreen(object sender, ScreenManager.ToggleAnyScreenEventArgs e) 
        {
            if (e.screenName == ScreenName) 
            {
                Debug.Log($"{e.screenName} - {e.active}");
                CheckGameManagerCallbacks();

                ToggleGraphicContainer(e.active);

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
            if (_anim != null && _performAnimationOnClose)
                _anim.SetTrigger(_closeTrigger);

            yield return new WaitForSeconds(delayToClose);

            ScreenManager.instance.ToggleScreenByTag(ScreenName, false);
        }

        private void ToggleGraphicContainer(bool active) 
        {
            if (_graphicContainer == null) 
            {
                Debug.LogWarning(gameObject.name + ": graphicContainer is null!");
                return;
            }

            _graphicContainer.SetActive(active);
            GraphicIsActive = active;
        }

        protected virtual void OnScreenEnable()
        {
            return; 
        }

        protected virtual void OnScreenDisabled()
        {
            return; 
        }

        public virtual void ReturnToPreviousScreen() 
        {
            ScreenManager.instance.ToggleScreenByTag(_previousScreenName, true);
            CloseScreen(_previousScreenTransitionDelay);
        }
    }
}
