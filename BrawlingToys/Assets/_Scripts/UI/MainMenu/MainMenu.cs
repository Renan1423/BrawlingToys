using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;

namespace BrawlingToys.UI
{
    public class MainMenu : BaseScreen
    {
        public void StartGame()
        {
            LevelManager.instance.LoadNextLevel();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
