using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Feedbacks;

namespace BrawlingToys.Actors
{
    public class Player : NetworkBehaviour
    {
        public static List<Player> Instances = new();
        
        /// <summary>
        /// A classe Player ser� o componente principal do objeto player(Pai) e funcionar� como um "container" dos principais atrubitos
        /// e componentes necess�rios para o funcionamento do objeto como um todo.
        /// </summary>
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
        public PlayerWeapon Weapon { get => _weapon; }
        public MMFeedbacks ShootFeedback { get => _shootFeedback; }
        [field: SerializeField] public int DashCount { get; set; }

        #endregion

        [SerializeField] private BaseStats _baseStatsSO;
        [SerializeField] private PlayerInputs _inputs;
        [SerializeField] private PlayerCooldownController _cooldowns;
        [SerializeField] private Stats _stats;
        [SerializeField] private Player _myKiller;
        [SerializeField] private Rigidbody _rb;
        
        [Header("State Stuffs: ")]
        [SerializeField] private StateFactory _stateFactory;
        [SerializeField] private State _currentState = null;
        [SerializeField] private State _previousState = null;

        [Header("Weapon Stuff: ")]
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private Transform _firePoint; // Instancia a bala nesse game object
        [SerializeField] private MMFeedbacks _shootFeedback;
        [SerializeField] private NetworkWeaponShooter _networkWeaponShooter; 
        private RaycastHit _hitInfo;
        private float _aimSmoothRate = 50f;
        private PlayerWeapon _weapon;

        private StatsMediator _mediator;

        [Header("Animation Settings")]
        [SerializeField] private GameObject _animationsGameObject;
        [SerializeField] private PlayerSpawnSelectedModel _spawnSelectedModel;

        [Header("Gizmos Stuff: ")]
        [SerializeField] private Color _aimColor;

        public bool canModify = false;
        public ModifierScriptable modifier;

        private bool _initilized;
        private PlayerAnimations _animations;

        private void Awake()
        {
            _spawnSelectedModel.OnModelSpawed += SpawnSelectedModel_OnModelLoaded;
        }

        private void OnEnable()
        {
            Debug.Log($"Initilized: {_initilized}");
            if (!_initilized) return; 
            
            if(PlayerEnableOnDeathState())
            {
                var idle = StateFactory.GetState(StateFactory.StateType.Idle); 
                TransitionToState(idle); 
            }

            bool PlayerEnableOnDeathState() => 
            _currentState.GetType() == typeof(DieState);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _spawnSelectedModel.OnModelSpawed -= SpawnSelectedModel_OnModelLoaded;
        }

        private void SpawnSelectedModel_OnModelLoaded()
        {
            _stateFactory.InitializeStates(this);
            InitializePlayer();
            TransitionToState(_stateFactory.GetState(StateFactory.StateType.Idle));
        }

        public override void OnNetworkSpawn()
        {
            PlayerId = OwnerClientId; 
            
            var playerInstances = GameObject.FindObjectsOfType<Player>(); 
            Instances = playerInstances.ToList(); 
        }

        private void Update()
        {
            if (_currentState == null)
                return;
            _currentState.UpdateState();

            _stats.Mediator.Update(Time.deltaTime);

            _cooldowns.UpdateCooldowns();

            if (Application.isFocused && IsOwner)
                _weapon.Update();
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
            DashCount = 0;
            _animations = GetComponentInChildren<PlayerAnimations>();
            _mediator = new();
            _stats = new(_mediator, _baseStatsSO);

            _cooldowns = new(this);
            _cooldowns.Initialize();

            _weapon = new(this, _firePoint, _aimSmoothRate, _groundLayerMask, _networkWeaponShooter);

            if(canModify)
                ApplyModifier();

            _initilized = true; 
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

        public void ApplyModifier()
        {
            _stats.ModifyStat(modifier, 1);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _aimColor;
            Gizmos.DrawSphere(_hitInfo.point, .5f);
        }
    }
}
