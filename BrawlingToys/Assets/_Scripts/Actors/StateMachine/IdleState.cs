using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected override void EnterState()
    {
        _player._animations.PlayAnimation(PlayerAnimations.AnimationType.Idle);
    }

    public override void UpdateState()
    {
        // Atualizar direção de mira
        // Lidar com Attack (shoot / melee)
        // Lidar com Dash
    }

    protected override void HandleMovement(object sender, Vector2 inputVector)
    {
        if(Mathf.Abs(inputVector.x) > 0f || Mathf.Abs(inputVector.y) > 0f)
        {
            _player.TransitionToState(_player._stateFactory.GetState(StateFactory.StateType.Movement));
        }
    }
}
