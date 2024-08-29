using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : State
{
    protected override void EnterState()
    {
        _player._animations.PlayAnimation(PlayerAnimations.AnimationType.MeleeAttack);
        _player._animations.OnAnimationEnd.AddListener(WhenMeleeEnds);

        // Aplicar força no player na direção de movimento
    }

    protected override void ExitState()
    {
        _player._animations.ResetEvents();
    }

    public override void UpdateState()
    {
        // Logica de verificação se acertou algo no caminho:
        // Se for bullet : aplica parry,
        // Se for player : aplica knockback;
        _player._meleeCommand.Execute();
    }

    protected override void HandleAttack(object sender, EventArgs e)
    {
        // Previne atacar durante um dash.
    }

    protected override void HandleDash(object sender, EventArgs e)
    {
        // Previne de dar outro dashe antes do término de um.
    }

    private void WhenMeleeEnds()
    {
        _player.TransitionToState(_player._stateFactory.GetState(StateFactory.StateType.Idle));
    }
}
