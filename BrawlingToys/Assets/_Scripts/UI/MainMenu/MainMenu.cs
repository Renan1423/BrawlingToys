using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;
using BrawlingToys.Core;

namespace BrawlingToys.UI
{
    public class MainMenu : BaseScreen
    {
        [Space(10)]

        [SerializeField]
        private MainMenuCameraAnimationHandler _mainMenuCamAnimHandler;

        protected override void OnScreenEnable()
        {
            _mainMenuCamAnimHandler.TriggerMainMenu();
        }

        public void StartGame()
        {
            _mainMenuCamAnimHandler.TriggerPlayGame();

            StartCoroutine(StartGameWithDelay());
        }

        private IEnumerator StartGameWithDelay() 
        {
            yield return new WaitForSeconds(1f);

            LevelManager.instance.LoadNextLevel();
            CloseScreen(0f);
        }

        public void OpenCredits() 
        {
            ScreenManager.instance.ToggleScreenByTag(TagManager.MainMenu.CREDITS, true);
            _mainMenuCamAnimHandler.TriggerCredits();
            CloseScreen(0f);
        }

        public void OpenSettings() 
        {
            CloseScreen(0f);
            _mainMenuCamAnimHandler.TriggerSettings();
        }

        public void OpenExitQuestion() 
        {
            QuestionScreen questionScreen = GameplayUiContainer.instance.QuestionScreen;
            questionScreen.gameObject.SetActive(true);
            questionScreen.InitQuestion("Você quer fechar o jogo?", QuitGame, CloseQuestionScreen);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void CloseQuestionScreen() { }
    }
}
