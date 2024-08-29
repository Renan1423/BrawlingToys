using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    protected virtual void HandleAttack(object sender, System.EventArgs e)
    {
        // Mudar as condi��es de acordo com o PCC!

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
        // Mudar as condi��es de acordo com o PCC!

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
