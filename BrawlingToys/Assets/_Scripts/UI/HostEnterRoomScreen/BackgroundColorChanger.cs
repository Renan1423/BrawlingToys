using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BrawlingToys.UI
{
    public class BackgroundColorChanger : MonoBehaviour
    {
        [SerializeField]
        private Image _backgroundImage;
        private Color _initialColor;

        private void Awake()
        {
            _initialColor = _backgroundImage.color;
        }

        public void SetBackgroundColor(Color newColor, float transitionTime)
        {
            _backgroundImage.DOColor(newColor, transitionTime);
        }

        public void ResetBackgroundColor(float transitionTime) 
        {
            _backgroundImage.DOColor(_initialColor, transitionTime);
        }
    }
}
