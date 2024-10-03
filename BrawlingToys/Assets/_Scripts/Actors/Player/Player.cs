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
        public Rigidbody Rig { get => _rig; }
        public StateFactory StateFactory { get => _stateFactory; }
        public State CurrentState { get => _currentState; }

        #endregion

        [SerializeField] private BaseStats _baseStatsSO;
        [SerializeField] private PlayerInputs _inputs;
        [SerializeField] private PlayerAnimations _animations;
        [SerializeField] private PlayerCooldownController _cooldowns;
        [SerializeField] private Stats _stats;
        [SerializeField] private Player _myKiller;
        [SerializeField] private Rigidbody _rig;
        //public PlayerWeapon weapon;
        
        [Header("State Stuffs: ")]
        [SerializeField] private StateFactory _stateFactory;
        [SerializeField] private State _currentState = null;
        [SerializeField] private State _previousState = null;

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
        [SerializeField] private Color _aimColor;

        private void Awake()
        {
            _stateFactory.InitializeStates(this);
        }

        private void OnEnable()
        {
            InitializePlayer();
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
                HandleAim();

            if (_knockback.Timer.IsRunning)
            {
                if (IsOwner)
                {
                    _inputs.TogglePlayerMap(false);
                    _knockback.AddForce(transform, _knockbackDirection, _knockbackPower, Time.deltaTime);
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

            _baseStatsSO.defaultHitEffect = new KillCommand();
            _mediator = new StatsMediator();
            _stats = new Stats(_mediator, _baseStatsSO);

            _cooldowns = new PlayerCooldownController(this);
            _cooldowns.Initialize();

            _knockback = new(_knockbackDuration);
            
            if (IsOwner)
            {
                _knockback.Timer.OnTimerStop += _inputs.EnablePlayerMap;
            }
    
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

        // Esse método vai ficar assim e a gente gerencia a invencibilidade no outro script (State)
        public void Damage(Player sender)////////////////////
        {
            Debug.Log("Damage");
            _myKiller = sender;
            DieServerRpc(); 
        }

        public void Knockback(GameObject sender)//////////////////////
        {
            KnockBackServerRpc(sender.transform.forward); 
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _aimColor;
            Gizmos.DrawSphere(hitInfo.point, .5f);
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
    }
}
