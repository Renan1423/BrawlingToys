using BrawlingToys.Network;
using UnityEngine;
using UnityEngine.Events;

namespace BrawlingToys.Managers
{
    public class GameManager : NetworkSingleton<GameManager>
    {
        [field:SerializeField]
        public GameStateType CurrentGameState { get; private set; }

        [HideInInspector]
        public UnityEvent<GameStateType> OnGameStateChange = new();

        public void ChangeGameState(GameStateType newGameState)
        {
            if(CurrentGameState == newGameState) return;

            CurrentGameState = newGameState;
            OnGameStateChange?.Invoke(newGameState);
        }
    }
}
