using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class DieState : State
    {
        protected override void EnterState()
        {
            if (_player.IsOwner)
            {
                _player._inputs.TogglePlayerMap(false);
            }

            _player.OnPlayerDeath?.Invoke(_player);
            _player.OnPlayerKill?.Invoke(_player.myKiller);

            _player._animations.PlayAnimation(PlayerAnimations.AnimationType.Die);
            _player._animations.OnAnimationEnd.AddListener(WhenAnimationEnd);
        }

        protected override void ExitState()
        {
            _player._animations.ResetEvents();
        }

        protected override void HandleShoot(object sender, EventArgs e)
        {
            // Previne de atirar durante a morte.
        }

        protected override void HandleMelee(object sender, EventArgs e)
        {
            // Previne ataque melee durante a morte.
        }

        protected override void HandleDash(object sender, EventArgs e)
        {
            // Previne dar dash
        }

        public override void HandleDie()
        {
            // Previne de entrar no estado de morte novamente
        }

        private void WhenAnimationEnd()
        {
            _player.gameObject.SetActive(false);
        }
    }
}
