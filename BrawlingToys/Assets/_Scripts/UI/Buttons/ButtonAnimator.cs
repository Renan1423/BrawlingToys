using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BrawlingToys.UI
{
    public class ButtonAnimator : MonoBehaviour
    {
        [Header("Image")]
        [SerializeField]
        private Image _buttonImage;

        [Space(10)]

        [Header("Colors")]

        [SerializeField]
        private Color _buttonNormalColor = Color.white;
        [SerializeField]
        private Color _buttonSelectedColor = Color.white;
        [SerializeField]
        private Color _buttonPressedMultiplyColor = Color.white;
        [SerializeField]
        private Color _buttonDisabledColor = Color.white;

        public void ChangeToNormalColor() 
        {
            _buttonImage.DOColor(_buttonNormalColor, 0.1f);
        }

        public void ChangeToSelectedColor() 
        {
            _buttonImage.DOColor(_buttonSelectedColor, 0.1f);
        }

        public void ApplyPressedMultiplyColor() 
        {
            _buttonImage.color += _buttonPressedMultiplyColor;
            _buttonImage.DOColor(_buttonNormalColor, 0.1f);
        }

        public void ChangeToDisabledColor() 
        {
            _buttonImage.DOColor(_buttonDisabledColor, 0.1f);
        }
    }
}
