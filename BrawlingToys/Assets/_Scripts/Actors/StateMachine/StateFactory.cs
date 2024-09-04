using UnityEngine;

namespace BrawlingToys.Actors
{
    public class StateFactory : MonoBehaviour
    {
        /// <summary>
        /// A classe StateFactory ficará responsável por armazenar todas as referências dos estados concretos e retorna-los,
        /// através do enum StateType, quando requisitado pelo Player.
        /// </summary>

        public enum StateType
        {
            Idle,
            Movement,
            MeleeAttack,
            Dash,
            Die
        }

        [SerializeField] private State _idle, _movement, _meleeAttack, _dash, _die;

        public State GetState(StateType stateType) => stateType switch
        {
            StateType.Idle => _idle,
            StateType.Movement => _movement,
            StateType.MeleeAttack => _meleeAttack,
            StateType.Dash => _dash,
            StateType.Die => _die,
            _ => throw new System.Exception("State not defined " + stateType.ToString())
        };

        // NOTA : Dessa forma é necessário que todos estados concretos sejam componentes do mesmo objeto. Talvez não seja a melhor solução.
        public void InitializeStates(Player player)
        {
            State[] states = GetComponents<State>();
            foreach (var state in states)
            {
                state.InitializeState(player);
            }
        }
    }
}
