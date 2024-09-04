using IngameDebugConsole;
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
    }
}
