using BrawlingToys.Managers;
using IngameDebugConsole;
using System;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.DevTools
{
    public class NetworkCamands 
    {
        [ConsoleMethod("ping", "Check if the framework is working")]
        public static void Ping()
        {
            Debug.Log("Pong");
        }

        [ConsoleMethod("start-host", "Inicia um host local. Caso queria jogar com outra máquina  usando o relay, use o camando start-party")]
        public static void StartHost()
        {
            NetworkManager.Singleton.StartHost(); 
            Debug.Log("Starting Host");
        }

        [ConsoleMethod("start-client", "Se conecta a um host local. Caso queria jogar com outra máquina usando o relay, use o camando join-party")]
        public static void StartClinet()
        {
            NetworkManager.Singleton.StartClient(); 
            Debug.Log("Starting Clinet");
        }

        [ConsoleMethod("change-game-state", "Muda o estado do GameManager, estados disponiveis: Connection, Building, Combat, Score, FinalScreen")]
        public static void ChangeGameState(string gameState)
        {
            var parseEnum = (GameStateType)Enum.Parse(typeof(GameStateType), gameState);
            GameManager.LocalInstance.ChangeGameState(parseEnum);
        }

        [ConsoleMethod("change-screen", "Call screen manager change")]
        public static void ChangeScreen(string screenName, bool active)
        {
            ScreenManager.instance.ToggleScreenByTag(screenName, active);
        }
    }
}
