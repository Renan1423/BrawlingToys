using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class DashState : State
    {
        [SerializeField] private Rigidbody _rig; 
        
        [Space]
        
        [SerializeField] float impulsePower = 20f;

        private Vector3 dashDirection;

        protected override void EnterState()
        {
            _player.Animations.PlayAnimation(PlayerAnimations.AnimationType.Dash);
            _player.Cooldowns.dashTimer.Start();

            // Aplicar for�a no player na dire��o de movimento
            float movementMagnitude = _player.Inputs.GetMovementVectorNormalized().magnitude;
            Vector3 movementDirection = new Vector3(_player.Inputs.GetMovementVectorNormalized().x, 0,
                _player.Inputs.GetMovementVectorNormalized().y);

            dashDirection = movementMagnitude > 0 ? movementDirection : Vector3.forward;

            _player.Animations.OnAnimationAction.AddListener(() => _player.Rig.AddForce(impulsePower * dashDirection, ForceMode.Impulse));
            _player.Animations.OnAnimationEnd.AddListener(WhenDashEnds);
        }

        protected override void ExitState()
        {
            _player.Rig.velocity = Vector3.zero;
            dashDirection = Vector3.zero;
            _player.Animations.ResetEvents();
        }

        protected override void HandleShoot(object sender, EventArgs e)
        {
            // Previne de atirar durante um dash.
        }

        protected override void HandleMelee(object sender, EventArgs e)
        {
            // Previne ataque melee durante um dash.
        }

        protected override void HandleDash(object sender, EventArgs e)
        {
            // Previne de dar outro dashe antes do t�rmino de um.
        }

        private void WhenDashEnds()
        {
            _player.TransitionToState(_player.StateFactory.GetState(StateFactory.StateType.Idle));
        }
    }
}
