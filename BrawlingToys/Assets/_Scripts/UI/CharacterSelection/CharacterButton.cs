using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrawlingToys.UI
{
    public class CharacterButton : MonoBehaviour
    {
        [SerializeField]
        private Image _characterIconImage;

        private int _characterIndex = 0;
        private CharacterSelectionScreen _selectionScreen;

        public void SetupCharacterButton(int characterIndex, Sprite characterIcon, CharacterSelectionScreen selectionScreen)
        {
            _characterIndex = characterIndex;
            _selectionScreen = selectionScreen;
            _characterIconImage.sprite = characterIcon;
        }

        public void OnClickCharacter() 
        {
            _selectionScreen.SelectCharacter();
        }

        public void OnHoverCharacter() 
        {
            _selectionScreen.ShowCharacter(_characterIndex);
        }
    }
}
