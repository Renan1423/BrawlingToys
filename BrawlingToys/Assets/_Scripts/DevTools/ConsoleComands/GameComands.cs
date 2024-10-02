using BrawlingToys.Managers;
using IngameDebugConsole;
using UnityEngine;

namespace BrawlingToys.DevTools
{
    public class GameComands 
    {
        [ConsoleMethod("next-state", "Passa para o proximo estado da gameplay")]
        public static void PassNextState()
        {
            var currentStateIndex = (int) GameManager.LocalInstance.CurrentGameState; 
            var nextStateIndex = currentStateIndex + 1; 

            var nextState = (GameStateType) nextStateIndex; 

            GameManager.LocalInstance.ChangeGameState(nextState); 

            Debug.Log($"Novo estado: {nextState.ToString()}");
        }

        [ConsoleMethod("change-screen", "Call screen manager change")]
        public static void ChangeScreen(string screenName, bool active)
        {
            ScreenManager.instance.ToggleScreenByTag(screenName, active);
        }
    }
}
