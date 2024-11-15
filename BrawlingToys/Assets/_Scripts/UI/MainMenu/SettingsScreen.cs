using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Core;
using BrawlingToys.Managers;

namespace BrawlingToys.UI
{
    public class SettingsScreen : BaseScreen
    {
        [SerializeField]
        private Animator _blackboardAnim;
        [SerializeField]
        private DrawerButton _settingsDrawerButton;

        [SerializeField]
        private GameObject[] _tabs;
        private int tabIndex = 0;

        protected override void OnScreenEnable()
        {
            _blackboardAnim.SetBool("ShowBlackboard", true);
        }

        protected override void OnScreenDisabled()
        {
            _blackboardAnim.SetBool("ShowBlackboard", false);
        }

        public void CloseSettings() 
        {
            CloseScreen(0f);
            Invoke(nameof(OpenMainMenu), 1.1f);
        }

        private void OpenMainMenu()
        {
            _settingsDrawerButton.OnHoverDrawer(false);
            _blackboardAnim.SetTrigger("DrawerClosed");
            ScreenManager.instance.ToggleScreenByTag(TagManager.MainMenu.MAIN_MENU, true);
        }

        public void ChangeTab(int incrementation) 
        {
            ToggleTab(tabIndex, false);

            tabIndex += incrementation;
            if (tabIndex < 0)
                tabIndex = _tabs.Length - 1;
            else if (tabIndex >= _tabs.Length)
                tabIndex = 0;

            ToggleTab(tabIndex, true);
        }

        private void ToggleTab(int index, bool result) 
        {
            _tabs[index].gameObject.SetActive(result);
        }
    }
}
