using UnityEngine;
using UnityEngine.Events;

namespace BrawlingToys.Actors
{
    public abstract class State : MonoBehaviour
    {
        public UnityEvent OnEnter, OnExit;

        protected Player _player;

        // Inicializa qualquer estado passando a refer�ncia do player.
        public void InitializeState(Player player)
        {
            _player = player;
        }

        /// <summary>
        ///  - Enter() e Exit() s�o metodos padr�es de entrada e sa�da que TODO estado concreto dever� realizar dessa mesma forma.
        /// Eles s�o respons�veis pela inscri��o dos Handlers nos eventos disparados pelo PlayerInputs
        /// 
        ///  - Os UnityEvent OnEnter e OnExit s�o opcionais, mas s�o �teis para logicas extras adicionadas pelo inspector.
        ///  NOTA : Adicionar apenas metodos de dentro da pr�pria hierarquia.
        /// </summary>
        public void Enter()
        {
            _player.Inputs.OnMoveAction += HandleMovement;
            _player.Inputs.OnShootAction += HandleShoot;
            _player.Inputs.OnMeleeAction += HandleMelee;
            _player.Inputs.OnDashAction += HandleDash;

            OnEnter?.Invoke();

            EnterState();
        }

        public void Exit()
        {
            _player.Inputs.OnMoveAction -= HandleMovement;
            _player.Inputs.OnShootAction -= HandleShoot;
            _player.Inputs.OnMeleeAction -= HandleMelee;
            _player.Inputs.OnDashAction -= HandleDash;

            OnExit?.Invoke();

            ExitState();
        }

        /// <summary>
        ///  - EnterState() e ExitState() s�o os metodos que cada estado concreto poder� reescrever com sua pr�pria l�gica / m�todos.
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
        ///  - Os Handlers responder�o aos inputs. E, cada estado concreto poder� reescreve-los para responder da forma que for necess�ria
        ///  em cada estado concreto.
        /// </summary>
        protected virtual void HandleMovement(object sender, Vector2 inputVector)
        {
        }

        protected virtual void HandleShoot(object sender, System.EventArgs e)
        {
            if (_player.Cooldowns.reloadTimer.IsRunning)
            {
                Debug.Log("Reload");
                return;
            }
                
            if (_player.Cooldowns.fireRateTimer.IsRunning)
            {
                Debug.Log("Fire-Rate...");
                return;
            }

            _player.Weapon.Shoot(_player.PlayerId);
            _player.ShootFeedback.PlayFeedbacks();

            _player.Cooldowns.fireRateTimer.Start();
            if (_player.Stats.ReloadTime <= 0.05f)
                return;
            _player.Cooldowns.reloadTimer.Start();
        }

        protected virtual void HandleMelee(object sender, System.EventArgs e)
        {
            if(_player.Cooldowns.meleeTimer.IsRunning)
            {
                // Som de fail
                Debug.Log("Melee est� em cooldown");
            }
            else
            {
                Debug.Log("State.HandleMelee");
                _player.TransitionToState(_player.StateFactory.GetState(StateFactory.StateType.MeleeAttack));
            }
        }

        protected virtual void HandleDash(object sender, System.EventArgs e)
        {
            if (_player.Cooldowns.dashTimer.IsRunning)
            {
                // Som de fail
                if (_player.Stats.DashAmount > 1) {
                    return;
                }
                Debug.Log("Dash est� em cooldown");
            }
            else
            {
                _player.TransitionToState(_player.StateFactory.GetState(StateFactory.StateType.Dash));
            }
        }

        public virtual void HandleDie()
        {
            // Checar imortalidade :

            _player.TransitionToState(_player.StateFactory.GetState(StateFactory.StateType.Die));
        }
    }
}
