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
        private Vector2 _keyInputSize = new Vector2(160f, 80f);
        [SerializeField]
        private Vector2 _gamepadInputSize = new Vector2(80f, 80f);

        private RectTransform rectTrans;

        private void OnEnable()
        {
            StartCoroutine(SetupDelegates());
        }

        private void OnDisable()
        {
            InputController.instance.OnGamepadUsed -= OnGamepadUsed;
            InputController.instance.OnKeyboardUsed -= OnKeyboardUsed;
        }

        private IEnumerator SetupDelegates() 
        {
            yield return new WaitForSeconds(0.01f);

            InputController.instance.OnGamepadUsed += OnGamepadUsed;
            InputController.instance.OnKeyboardUsed += OnKeyboardUsed;

            if (InputController.instance.IsUsingGamepad)
                OnGamepadUsed();
            else
                OnKeyboardUsed();
        }

        private void OnGamepadUsed() 
        {
            _sprites = InputController.instance.inputsDictionary[_gamepadCode];

            rectTrans = _spr.GetComponent<RectTransform>();
            rectTrans.sizeDelta = _gamepadInputSize;
        }

        private void OnKeyboardUsed() 
        {
            _sprites = InputController.instance.inputsDictionary[_keyCode];

            rectTrans = _spr.GetComponent<RectTransform>();
            rectTrans.sizeDelta = _keyInputSize;
        }

        private void Start()
        {
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