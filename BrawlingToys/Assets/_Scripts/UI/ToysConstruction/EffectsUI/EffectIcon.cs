using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BrawlingToys.UI
{
    public class EffectIcon : MonoBehaviour
    {
        [SerializeField]
        private GameObject _descriptionBalloon;
        [SerializeField]
        private TextMeshProUGUI _effectNameText;
        [SerializeField]
        private TextMeshProUGUI _descriptionText;
        [SerializeField]
        private Image _effectImage;

        public void SetupIcon(string name, string description, Sprite effectSprite)
        {
            _effectNameText.text = name;
            _descriptionText.text = description;
            _effectImage.sprite = effectSprite;
        }

        public void ToggleDescription(bool result)
        {
            _descriptionBalloon.SetActive(result);
        }
    }
}