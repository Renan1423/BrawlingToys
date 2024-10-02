using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class MeleeAttackState : State
    {
        protected override void EnterState()
        {
            _player.Animations.PlayAnimation(PlayerAnimations.AnimationType.MeleeAttack);
            _player.Animations.OnAnimationEnd.AddListener(WhenMeleeEnds);
            _player.Cooldowns.meleeTimer.Start();

            // Aplicar for�a no player na dire��o de movimento
        }

        protected override void ExitState()
        {
            _player.Animations.ResetEvents();
        }

        public override void UpdateState()
        {
            // Logica de verifica��o se acertou algo no caminho:
            // Se for bullet : aplica parry,
            // Se for player : aplica knockback;
            _player._meleeCommand.Execute();
        }

        protected override void HandleShoot(object sender, EventArgs e)
        {
            // Previne atirar durante um melee.
        }

        protected override void HandleMelee(object sender, EventArgs e)
        {
            // Previne ataque melee durante um melee.
        }

        protected override void HandleDash(object sender, EventArgs e)
        {
            // Previne de dar outro dashe antes do t�rmino de um.
        }

        private void WhenMeleeEnds()
        {
            _player.TransitionToState(_player.StateFactory.GetState(StateFactory.StateType.Idle));
        }
    }
}
