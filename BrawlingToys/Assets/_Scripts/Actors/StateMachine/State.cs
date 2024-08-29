using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        _player._inputs.OnAttackAction += HandleAttack;
        _player._inputs.OnDashAction += HandleDash;

        OnEnter?.Invoke();

        EnterState();
    }

    public void Exit()
    {
        _player._inputs.OnMoveAction -= HandleMovement;
        _player._inputs.OnAttackAction -= HandleAttack;
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

    protected virtual void HandleAttack(object sender, System.EventArgs e)
    {
        // Mudar as condições de acordo com o PCC!

        if (true)
        {
            _player._shootCommand?.Execute();
        }
        else if(false)
        {
            _player.TransitionToState(_player._stateFactory.GetState(StateFactory.StateType.MeleeAttack));
        }
        else
        {
            // Adicionar um evento som tiq tiq;
        }
    }

    protected virtual void HandleDash(object sender, System.EventArgs e)
    {
        // Mudar as condições de acordo com o PCC!

        if (true)
        {
            _player.TransitionToState(_player._stateFactory.GetState(StateFactory.StateType.Dash));
        }
        else
        {
            // Adicionar evento som toq toq
        }
    }

    public virtual void HandleDie() 
    {
        // Checar imortalidade :

        _player.TransitionToState(_player._stateFactory.GetState(StateFactory.StateType.Die));
    }
}
