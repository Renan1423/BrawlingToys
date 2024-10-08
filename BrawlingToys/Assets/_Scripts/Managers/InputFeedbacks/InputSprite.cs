using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrawlingToys.Managers
{
    public class InputSprite : MonoBehaviour
    {
        [SerializeField]
        private KeyCode _keyCode;
        [SerializeField]
        private KeyCode _gamepadCode;
        [SerializeField]
        private Image _spr;
        private Sprite[] _sprites;

        [SerializeField] 
        private float _animationSpeed = 5f;
        private float _animationTimeCount;
        private float _animationTimeLimit = 10f;
        private int _animationIndex = 0;
        [SerializeField]
        private Vector2 _inputSize = new Vector2(9f, 9f);

        private RectTransform rectTrans;

        private void OnEnable()
        {
            InputController.instance.OnGamepadUsed += OnGamepadUsed;
            InputController.instance.OnKeyboardUsed += OnKeyboardUsed;
        }

        private void OnGamepadUsed() 
        {
            _sprites = InputController.instance.inputsDictionary[_gamepadCode];
        }

        private void OnKeyboardUsed() 
        {
            _sprites = InputController.instance.inputsDictionary[_keyCode];
        }

        private void Start()
        {
            rectTrans = GetComponent<RectTransform>();
            rectTrans.sizeDelta = _inputSize;

            _animationIndex = 0;

            if (_sprites != null)
            {
                _spr.sprite = _sprites[0];
                _animationIndex = 0;
            }

        }

        private void Update()
        {
            if (_sprites == null)
                return;

            _animationTimeCount += _animationSpeed * Time.deltaTime;

            if (_animationTimeCount >= _animationTimeLimit)
            {
                _animationTimeCount = 0f;
                AnimateInput();
            }
        }

        private void AnimateInput()
        {
            _animationIndex++;
            if (_animationIndex >= _sprites.Length)
                _animationIndex = 0;

            _spr.sprite = _sprites[_animationIndex];
        }
    }
}