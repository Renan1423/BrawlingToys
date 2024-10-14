using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.DesignPatterns;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

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

        public bool IsUsingGamepad { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            FillDictionary();

            IsUsingGamepad = (Gamepad.current != null);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (Gamepad.current == null)
                OnKeyboardUsed?.Invoke();
            else
                OnGamepadUsed?.Invoke();
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
            if (Gamepad.current == null)
                return;

            bool gamepadButtonPressed = Gamepad.current.allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic);
            if (gamepadButtonPressed) 
            {
                OnGamepadUsed?.Invoke();
                IsUsingGamepad = true;
            }
        }

        private void DetectKeyboardInput() 
        {
            if (Keyboard.current == null)
                return;

            bool keyboardButtonPressed = Keyboard.current.anyKey.wasPressedThisFrame;
            if (keyboardButtonPressed) 
            {
                OnKeyboardUsed?.Invoke();
                IsUsingGamepad = false;
            }

        }
    }
}