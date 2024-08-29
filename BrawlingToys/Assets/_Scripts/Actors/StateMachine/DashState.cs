using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : State
{
    protected override void EnterState()
    {
        _player._animations.PlayAnimation(PlayerAnimations.AnimationType.Dash);
        _player._animations.OnAnimationEnd.AddListener(WhenDashEnds);

        // Aplicar força no player na direção de movimento
    }

    protected override void ExitState()
    {
        _player._animations.ResetEvents();
    }

    protected override void HandleAttack(object sender, EventArgs e)
    {
        // Previne de atacar durante um dash.
    }

    protected override void HandleDash(object sender, EventArgs e)
    {
        // Previne de dar outro dashe antes do término de um.
    }

    private void WhenDashEnds()
    {
        _player.TransitionToState(_player._stateFactory.GetState(StateFactory.StateType.Idle));
    }
}
