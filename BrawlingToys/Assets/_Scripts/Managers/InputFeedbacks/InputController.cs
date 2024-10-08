using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.DesignPatterns;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.InputSystem.Controls;

namespace BrawlingToys.Managers
{
    public class InputController : Singleton<InputController>
    {
        [System.Serializable]
        public struct Inputs
        {
            public KeyCode keyCode;
            public Sprite[] sprites;
        }
        public List<Inputs> inputs;

        public Dictionary<KeyCode, Sprite[]> inputsDictionary;

        public delegate void InputTypeUsed();
        public InputTypeUsed OnGamepadUsed;
        public InputTypeUsed OnKeyboardUsed;

        protected override void Awake()
        {
            base.Awake();
            FillDictionary();
        }

        private void FillDictionary()
        {
            inputsDictionary = new Dictionary<KeyCode, Sprite[]>();

            foreach (Inputs input in inputs)
            {
                inputsDictionary.Add(input.keyCode, input.sprites);
            }
        }

        private void Update()
        {
            DetectGamepadInput();
            DetectKeyboardInput();
        }

        private void DetectGamepadInput() 
        {
            bool gamepadButtonPressed = Gamepad.current.allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic);
            if (gamepadButtonPressed) 
                OnGamepadUsed?.Invoke();
        }

        private void DetectKeyboardInput() 
        {
            bool keyboardButtonPressed = Keyboard.current.anyKey.wasPressedThisFrame;
            if (keyboardButtonPressed)
                OnKeyboardUsed?.Invoke();
        }
    }
}