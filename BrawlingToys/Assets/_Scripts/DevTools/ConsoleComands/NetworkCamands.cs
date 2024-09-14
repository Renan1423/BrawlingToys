using BrawlingToys.Managers;
using BrawlingToys.Network;
using IngameDebugConsole;
using System;
using System.Linq;
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

        [ConsoleMethod("create-party","Cria a party para outros jogadores se conectarem")]
        public static async void CreateParty()
        {
            var operation = await RelayParty.Instance.CreatePartyAsync(); 
            
            if (operation.success)
            {
                var textEditor = new TextEditor
                {
                    text = operation.partyCode
                }; 

                textEditor.SelectAll(); 
                textEditor.Copy(); 
                
                Debug.Log($"Party criada com sucesso, codigo: {operation.partyCode}");
                Debug.Log("Codigo configurado para o CTRL C");
            }
            else
            {
                Debug.Log("Erro ao criar a party");
            }
        }

        [ConsoleMethod("join-party", "Se conecta a party de outro jogador")]
        public static async void JoinParty(string partyCode)
        {
            var success = await RelayParty.Instance.TryJoinPartyAsync(partyCode); 

            if (success)
            {   
                Debug.Log($"Sucesso ao entrar na party: {partyCode}");
            }
            else
            {
                Debug.Log($"Falha ao entrar na party: {partyCode}");
            } 
        }

        [ConsoleMethod("kill", "Mata um jogador na cena")]
        public static void KillPlayer(ulong playerId)
        {
            var player = MatchManager.LocalInstance.MatchPlayers.FirstOrDefault(p => p.PlayerId == playerId); 
            player.Damage(null); 
        }
    }
}
