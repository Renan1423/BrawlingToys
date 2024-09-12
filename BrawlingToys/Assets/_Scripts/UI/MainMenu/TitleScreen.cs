using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace BrawlingToys.UI
{
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject _mainMenu;

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
            _mainMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
