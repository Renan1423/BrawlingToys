using BrawlingToys.Network;
using UnityEngine;
using UnityEngine.Events;

namespace BrawlingToys.Managers
{
    public class GameManager : NetworkSingleton<GameManager>
    {
        public GameStateType CurrentGameState { get; private set; }

        [HideInInspector]
        public UnityEvent<GameStateType> OnGameStateChange = new();
        [SerializeField]
        private GameStateType _initialState = GameStateType.Combat;

        private void Start()
        {
            ChangeGameState(_initialState); 
        }

        public void ChangeGameState(GameStateType newGameState)
        {
            if(CurrentGameState == newGameState) return;

            CurrentGameState = newGameState;
            OnGameStateChange?.Invoke(newGameState);
        }
    }
}
