using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;
using BrawlingToys.Core;
using UnityEngine.UI;

namespace BrawlingToys.UI
{
    public class MainMenu : BaseScreen
    {
        [Space(10)]

        [SerializeField]
        private MainMenuCameraAnimationHandler _mainMenuCamAnimHandler;

        [SerializeField]
        private Button[] _drawerButtons;

        protected override void OnScreenEnable()
        {
            _mainMenuCamAnimHandler.TriggerMainMenu();

            ToggleDrawerButtons(true);
        }

        protected override void OnScreenDisabled()
        {
            ToggleDrawerButtons(false);
        }

        public void StartGame()
        {
            _mainMenuCamAnimHandler.TriggerPlayGame();

            StartCoroutine(StartGameWithDelay());
        }

        private IEnumerator StartGameWithDelay()
        {
            yield return new WaitForSeconds(2.5f);

            LevelManager.LocalInstance.LoadNextLevel();
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
            ScreenManager.instance.ToggleScreenByTag(TagManager.MainMenu.SETTINGS, true);
            _mainMenuCamAnimHandler.TriggerSettings();
            CloseScreen(0f);
        }

        public void OpenExitQuestion()
        {
            QuestionScreen questionScreen = GameplayUiContainer.instance.QuestionScreen;
            ScreenManager.instance.ToggleScreenByTag(TagManager.MainMenu.QUESTION_SCREEN, true);
            questionScreen.InitQuestion("Você quer fechar o jogo?", QuitGame, CloseQuestionScreen);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void CloseQuestionScreen() { }

        private void ToggleDrawerButtons(bool result) 
        {
            foreach (Button btn in _drawerButtons)
            {
                btn.gameObject.SetActive(result);
            }   
        }
    }
}
