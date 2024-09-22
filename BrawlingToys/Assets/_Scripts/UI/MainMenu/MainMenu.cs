using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;
using BrawlingToys.Core;

namespace BrawlingToys.UI
{
    public class MainMenu : BaseScreen
    {
        public void StartGame()
        {
            LevelManager.instance.LoadNextLevel();
        }

        public void OpenCredits() 
        {
            ScreenManager.instance.ToggleScreenByTag(TagManager.MainMenu.CREDITS, true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
