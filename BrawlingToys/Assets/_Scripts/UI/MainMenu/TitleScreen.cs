using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using BrawlingToys.Managers;
using BrawlingToys.Core;

namespace BrawlingToys.UI
{
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField]
        private Animator _anim;

        private void OnEnable()
        {
            InputSystem.onEvent += OnEvent;
        }

        private void OnDisable()
        {
            InputSystem.onEvent -= OnEvent;
        }

        private void OnEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
                return;
            var controls = device.allControls;
            var buttonPressPoint = InputSystem.settings.defaultButtonPressPoint;
            for (var i = 0; i < controls.Count; ++i)
            {
                var control = controls[i] as ButtonControl;
                if (control == null || control.synthetic || control.noisy)
                    continue;
                if (control.ReadValueFromEvent(eventPtr, out var value) && value >= buttonPressPoint)
                {
                    //Button Pressed
                    CloseTitleScreen();
                    break;
                }
            }
        }

        private void CloseTitleScreen() 
        {
            ScreenManager.instance.ToggleScreenByTag(TagManager.MainMenu.MAIN_MENU, true);
            _anim.SetTrigger("Close");

            StartCoroutine(CloseTitleScreenCoroutine());
        }

        private IEnumerator CloseTitleScreenCoroutine() 
        {
            yield return new WaitForSeconds(0.5f);

            this.gameObject.SetActive(false);
        }


    }
}
