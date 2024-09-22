using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.DesignPatterns;
using BrawlingToys.Managers;
using BrawlingToys.Core;

namespace BrawlingToys.UI
{
    public class CreditsScreen : BaseScreen
    {
        [Space(20)]

        [Header("Credits properties")]
        [SerializeField]
        private Animator _creditsScrollAnim;
        [SerializeField]
        private PlayerClickObserver _playerClickObserver;

        private void OnEnable()
        {
            _playerClickObserver.OnClick += OnPlayerClick;
        }

        private void OnDisable()
        {
            _playerClickObserver.OnClick -= OnPlayerClick;
        }

        private void OnPlayerClick(bool toggle) 
        {
            //If true, increase velocity of the credits scroll
            float scrollSpeed = (toggle) ? 5f : 1f;

            _creditsScrollAnim.speed = scrollSpeed;
        }

        public void CloseCredits() 
        {
            ScreenManager.instance.ToggleScreenByTag(TagManager.MainMenu.MAIN_MENU, true);   
            CloseScreen(0f);
        }
    }
}
