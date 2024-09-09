using System;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class Player : NetworkBehaviour, ICommandManager, IDamageable
    {
        /// <summary>
        /// A classe Player ser� o componente principal do objeto player(Pai) e funcionar� como um "container" dos principais atrubitos
        /// e componentes necess�rios para o funcionamento do objeto como um todo.
        /// </summary>
        public event EventHandler<Vector3> OnUpdateAimRotation;

        // NOTE : Talvez seja melhor fazer um metodo Get...() ou uma propriedade ao inv�s de deixar tudo publico.
        public BaseStats _baseStatsSO;
        public PlayerInputs _inputs;
        public PlayerAnimations _animations;
        public PlayerCooldownController _cooldowns;
        public Stats _stats;
        //public PlayerWeapon weapon;

        [Header("State Stuffs: ")]
        public StateFactory _stateFactory;
        public State _currentState = null, _previousState = null;

        [Header("Command Stuffs: ")]
        public ICommand _shootCommand;
        public ICommand _meleeCommand;
        [SerializeField] private float _meleeRadius;
        private bool bala;

        [Header("Aim Stuff: ")]
        [SerializeField] private LayerMask _groundLayerMask;
        private RaycastHit hitInfo;
        private float aimSmoothRate = 50f;

        [Header("Damage Stuffs: ")]
        [SerializeField] private Transform _firePoint; // Instancia a bala nesse game object
        [SerializeField] private float _knockbackDuration;
        [SerializeField] private float _knockbackPower;
        private Vector3 _knockbackDirection = Vector3.zero;
        private PhysicUtil _knockback;

        private StatsMediator _mediator;

        [Header("Gizmos Stuff: ")]
        [SerializeField] private Color _meleeColor;
        [SerializeField] private Color _aimColor;

        // Damageable padrão, retirar
        public int Health { get; set; }

        private void Awake()
        {
            _stateFactory.InitializeStates(this);
        }

        private void Start()
        {
            InitializePlayer();
        }

        private void Update()
        {
            if (_currentState == null)
                return;
            _currentState.UpdateState();

            _stats.Mediator.Update(Time.deltaTime);

            _cooldowns.UpdateCooldowns();

            if (Application.isFocused && IsOwner)
                HandleAim();

            if (_knockback.Timer.IsRunning)
            {
                if (IsOwner)
                {
                    _inputs.TogglePlayerMap(false);
                    _knockback.AddForce(transform, _knockbackDirection, _knockbackPower, Time.deltaTime);
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (bala)
                {
                    SetShootingCommand(new PushBulletCommand(_firePoint));
                    bala = !bala;
                }
                else
                {
                    SetShootingCommand(new KillBulletCommand(_firePoint));
                    bala = !bala;
                }
            }
        }

        private void FixedUpdate()
        {
            if (_currentState == null) return;

            _currentState.FixedUpdateState();
        }

        // Metodo que inicializar� todos componentes ou par�metros necess�rios do player como, seu estado inicial, estado inicial da arma 
        // e possivelmente modifica��es de buffs e debuffs.
        private void InitializePlayer()
        {
            TransitionToState(_stateFactory.GetState(StateFactory.StateType.Idle));

            _mediator = new StatsMediator();
            _stats = new Stats(_mediator, _baseStatsSO);

            _cooldowns = new PlayerCooldownController(this);
            _cooldowns.Initialize();

            SetShootingCommand(new PushBulletCommand(_firePoint));
            SetMeleeCommand(new MeleeCommand(_firePoint, _meleeRadius));

            _knockback = new(_knockbackDuration);
            
            if (IsOwner)
            {
                _knockback.Timer.OnTimerStop += _inputs.EnablePlayerMap;
            }
        }

        // Metodo respons�vel por toda a troca de estado. Chama o Exit() do atual e o Enter() do novo,
        // assim como a atualiza��o dos par�metros _currentState e _previousState.
        public void TransitionToState(State goalState)
        {
            if (goalState == null)
                return;

            if (_currentState != null)
                _currentState.Exit();

            _previousState = _currentState;
            _currentState = goalState;
            _currentState.Enter();
        }

        public void DieInCurrentState()
        {
            if (_currentState == null) return;

            _currentState.HandleDie();
        }

        private void HandleAim()
        {
            if (_knockback.Timer.IsRunning)
                return;

            Ray ray = Camera.main.ScreenPointToRay(_inputs.GetLookVector());
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, _groundLayerMask))
            {
                transform.forward = Vector3.Slerp(transform.forward, hitInfo.point - transform.position, aimSmoothRate * Time.deltaTime);
                OnUpdateAimRotation?.Invoke(this, transform.forward);
            }
        }

        public void SetShootingCommand(ICommand command)
        {
            _shootCommand = command;
        }

        public void SetMeleeCommand(ICommand command)
        {
            _meleeCommand = command;
        }

        // Esse método vai ficar assim e a gente gerencia a invencibilidade no outro script (State)
        public void Damage()
        {
            DieServerRpc(); 
        }

        public void Knockback(GameObject sender)
        {
            KnockBackServerRpc(sender.transform.forward); 
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _aimColor;
            Gizmos.DrawSphere(hitInfo.point, .5f);

            Gizmos.color = _meleeColor;
            Gizmos.DrawSphere(_firePoint.position, _meleeRadius);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DieServerRpc()
        {
            DieClientRpc(); 
        }

        [ClientRpc]
        private void DieClientRpc()
        {
            DieInCurrentState(); 
        }

        [ServerRpc(RequireOwnership = false)]
        private void KnockBackServerRpc(Vector3 senderForward)
        {
            KnockBackClientRpc(senderForward); 
        }

        [ClientRpc]
        private void KnockBackClientRpc(Vector3 senderForward)
        {
            _knockback.Timer.Start();
            _knockbackDirection = senderForward;
        }
    }
}
