using UnityEngine;
using UnityEngine.Events;

namespace BrawlingToys.Actors
{
    public abstract class State : MonoBehaviour
    {
        public UnityEvent OnEnter, OnExit;

        protected Player _player;

        // Inicializa qualquer estado passando a referência do player.
        public void InitializeState(Player player)
        {
            _player = player;
        }

        /// <summary>
        ///  - Enter() e Exit() são metodos padrões de entrada e saída que TODO estado concreto deverá realizar dessa mesma forma.
        /// Eles são responsáveis pela inscrição dos Handlers nos eventos disparados pelo PlayerInputs
        /// 
        ///  - Os UnityEvent OnEnter e OnExit são opcionais, mas são úteis para logicas extras adicionadas pelo inspector.
        ///  NOTA : Adicionar apenas metodos de dentro da própria hierarquia.
        /// </summary>
        public void Enter()
        {
            _player._inputs.OnMoveAction += HandleMovement;
            _player._inputs.OnShootAction += HandleShoot;
            _player._inputs.OnMeleeAction += HandleMelee;
            _player._inputs.OnDashAction += HandleDash;

            OnEnter?.Invoke();

            EnterState();
        }

        public void Exit()
        {
            _player._inputs.OnMoveAction -= HandleMovement;
            _player._inputs.OnShootAction -= HandleShoot;
            _player._inputs.OnMeleeAction -= HandleMelee;
            _player._inputs.OnDashAction -= HandleDash;

            OnExit?.Invoke();

            ExitState();
        }

        /// <summary>
        ///  - EnterState() e ExitState() são os metodos que cada estado concreto poderá reescrever com sua própria lógica / métodos.
        /// </summary>
        protected virtual void EnterState()
        {
        }

        protected virtual void ExitState()
        {
        }

        public virtual void UpdateState()
        {
        }

        public virtual void FixedUpdateState()
        {
        }

        /// <summary>
        ///  - Os Handlers responderão aos inputs. E, cada estado concreto poderá reescreve-los para responder da forma que for necessária
        ///  em cada estado concreto.
        /// </summary>
        protected virtual void HandleMovement(object sender, Vector2 inputVector)
        {
        }

        protected virtual void HandleShoot(object sender, System.EventArgs e)
        {
            if (_player._cooldowns.reloadTimer.IsRunning)
            {
                // Som de reload
                Debug.Log("Reload");
            }
            else
            {
                if (!_player._cooldowns.fireRateTimer.IsRunning)
                {
                    _player._shootCommand.Execute();
                    _player._cooldowns.reloadTimer.Start();
                }
            }
        }

        protected virtual void HandleMelee(object sender, System.EventArgs e)
        {
            if(_player._cooldowns.meleeTimer.IsRunning)
            {
                // Som de fail
                Debug.Log("Melee está em cooldown");
            }
            else
            {
                _player._meleeCommand.Execute();
            }
        }

        protected virtual void HandleDash(object sender, System.EventArgs e)
        {
            if (_player._cooldowns.dashTimer.IsRunning)
            {
                // Som de fail
                Debug.Log("Dash está em cooldown");
            }
            else
            {
                _player.TransitionToState(_player._stateFactory.GetState(StateFactory.StateType.Dash));
            }
        }

        public virtual void HandleDie()
        {
            // Checar imortalidade :

            _player.TransitionToState(_player._stateFactory.GetState(StateFactory.StateType.Die));
        }
    }
}
