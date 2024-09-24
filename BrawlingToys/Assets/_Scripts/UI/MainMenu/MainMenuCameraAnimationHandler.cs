using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.UI
{
    public class MainMenuCameraAnimationHandler : MonoBehaviour
    {
        [SerializeField]
        private Animator _anim;

        public void TriggerTitleScreen()
        {
            _anim.SetTrigger("TitleScreen");
        }

        public void TriggerMainMenu() 
        {
            _anim.SetTrigger("MainMenu");
        }

        public void TriggerSettings()
        {
            _anim.SetTrigger("Settings");
        }

        public void TriggerCloseMainMenu()
        {
            _anim.SetTrigger("CloseMenu");
        }
    }
}
