using System;
using Unity.Netcode;
using UnityEngine;
using BrawlingToys.Core;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using BrawlingToys.Network;

namespace BrawlingToys.Actors
{
    public class Player : NetworkBehaviour
    {
        public static List<Player> Instances = new();
        
        /// <summary>
        /// A classe Player ser� o componente principal do objeto player(Pai) e funcionar� como um "container" dos principais atrubitos
        /// e componentes necess�rios para o funcionamento do objeto como um todo.
        /// </summary>
        public event EventHandler<Vector3> OnUpdateAimRotation;
        public UnityEvent<Player> OnPlayerInitialize = new();
        public UnityEvent<Player> OnPlayerKill = new();
        public UnityEvent<Player> OnPlayerDeath = new();

        // NOTE : Talvez seja melhor fazer um metodo Get...() ou uma propriedade ao inv�s de deixar tudo publico.
        #region Properties
        public ulong PlayerId { get; private set; }
        public BaseStats BaseStatsSO { get => _baseStatsSO; }
        public PlayerInputs Inputs { get => _inputs; }
        public PlayerAnimations Animations { get => _animations; }
        public PlayerCooldownController Cooldowns { get => _cooldowns; }
        public Stats Stats { get => _stats; }
        public Player MyKiller { get => _myKiller; }
        public Rigidbody Rb { get => _rb; }
        public StateFactory StateFactory { get => _stateFactory; }
        public State CurrentState { get => _currentState; }

        #endregion

        [SerializeField] private BaseStats _baseStatsSO;
        [SerializeField] private PlayerInputs _inputs;
        [SerializeField] private PlayerAnimations _animations;
        [SerializeField] private PlayerCooldownController _cooldowns;
        [SerializeField] private Stats _stats;
        [SerializeField] private Player _myKiller;
        [SerializeField] private Rigidbody _rb;
        //public PlayerWeapon weapon;
        
        [Header("State Stuffs: ")]
        [SerializeField] private StateFactory _stateFactory;
        [SerializeField] private State _currentState = null;
        [SerializeField] private State _previousState = null;

        [Header("Weapon Stuff: ")]
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private Transform _firePoint; // Instancia a bala nesse game object
        private RaycastHit hitInfo;
        private float aimSmoothRate = 50f;

        private StatsMediator _mediator;

        [Header("Gizmos Stuff: ")]
        [SerializeField] private Color _aimColor;

        private void Awake()
        {
            _stateFactory.InitializeStates(this);
            InitializePlayer();
        }

        public override void OnNetworkSpawn()
        {
            PlayerId = OwnerClientId; 
            
            var playerInstances = GameObject.FindObjectsOfType<Player>(); 
            Instances = playerInstances.ToList(); 
        }

        private void Start()
        {
            TransitionToState(_stateFactory.GetState(StateFactory.StateType.Idle));
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
            _baseStatsSO.defaultHitEffect = new KillCommand();
            _mediator = new StatsMediator();
            _stats = new Stats(_mediator, _baseStatsSO);

            _cooldowns = new PlayerCooldownController(this);
            _cooldowns.Initialize();

            OnPlayerInitialize?.Invoke(this);
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

        private void HandleAim()
        {
            if (!_inputs.IsActive)
                return;

            Ray ray = Camera.main.ScreenPointToRay(_inputs.GetLookVector());
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, _groundLayerMask))
            {
                transform.forward = Vector3.Slerp(transform.forward, hitInfo.point - transform.position, aimSmoothRate * Time.deltaTime);
                OnUpdateAimRotation?.Invoke(this, transform.forward);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnBulletServerRpc(string bulletName, ulong bulletOwnerPlayerId)
        {
            var bullet = NetworkSpawner
                .LocalInstance
                .InstantiateOnServer(bulletName, _firePoint.position, _firePoint.rotation)
                .GetComponent<Bullet>();
                
            var owner = Instances.First(p => p.PlayerId == bulletOwnerPlayerId); 
            
            bullet.Initialize(owner); 
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _aimColor;
            Gizmos.DrawSphere(hitInfo.point, .5f);
        }
    }
}
