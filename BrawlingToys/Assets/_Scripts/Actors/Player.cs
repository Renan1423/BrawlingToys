using System;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, ICommandManager
{
    /// <summary>
    /// A classe Player ser� o componente principal do objeto player(Pai) e funcionar� como um "container" dos principais atrubitos
    /// e componentes necess�rios para o funcionamento do objeto como um todo.
    /// </summary>
    public event EventHandler<Vector3> OnUpdateAimRotation;

    // NOTE : Talvez seja melhor fazer um metodo Get...() ou uma propriedade ao inv�s de deixar tudo publico.
    public PlayerScriptable dataSO;
    public PlayerInputs _inputs;
    public PlayerAnimations _animations;
    //public PlayerWeapon weapon;

    [Header("State Stuffs: ")]
    public StateFactory _stateFactory;
    public State _currentState = null, _previousState = null;

    [Header("Command Stuffs: ")]
    public ICommand _shootCommand;
    public ICommand _meleeCommand;

    [SerializeField] private LayerMask _groundLayerMask;
    private Vector3 lookDirection;
    private RaycastHit hitInfo;
    private float aimSmoothRate = 50f;

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

        if(Application.isFocused && IsOwner)
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
        TransitionToState(_stateFactory.GetState(StateFactory.StateType.Idle));
        lookDirection = Vector3.zero;

        SetShootingCommand(new KillBulletCommand(transform));
        SetMeleeCommand(new MeleeCommand(transform));
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

    public void DieInCurrentState() //*
    {
        if (_currentState == null) return;

        _currentState.HandleDie();
    }

    private void HandleAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(_inputs.GetLookVector());
        if(Physics.Raycast(ray, out hitInfo, float.MaxValue, _groundLayerMask))
        {
            lookDirection = Vector3.Slerp(lookDirection, hitInfo.point - transform.position, aimSmoothRate * Time.deltaTime);
            OnUpdateAimRotation?.Invoke(this, lookDirection);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitInfo.point, .5f);
    }

    public void SetShootingCommand(ICommand command)
    {
        _shootCommand = command;
    }

    public void SetMeleeCommand(ICommand command)
    {
        _meleeCommand = command;
    }
}
