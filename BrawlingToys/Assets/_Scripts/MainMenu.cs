using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;

public class MainMenu : MonoBehaviour
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
