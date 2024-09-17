using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class DashState : State
    {
        [SerializeField] private Rigidbody _rig; 
        
        [Space]
        
        [SerializeField] float dashDuration = 0;
        [SerializeField] float impulsePower = 20f;

        private Vector3 dashDirection;

        private PhysicUtil physicUtil;

        protected override void EnterState()
        {
            _player._animations.PlayAnimation(PlayerAnimations.AnimationType.Dash);
            _player._cooldowns.dashTimer.Start();
            physicUtil = new PhysicUtil(dashDuration);

            // Aplicar for�a no player na dire��o de movimento
            float movementMagnitude = _player._inputs.GetMovementVectorNormalized().magnitude;
            Vector3 movementDirection = new Vector3(_player._inputs.GetMovementVectorNormalized().x, 0,
                _player._inputs.GetMovementVectorNormalized().y);

            dashDirection = movementMagnitude > 0 ? movementDirection : Vector3.forward;

            physicUtil.Timer.Start();
        }

        protected override void ExitState()
        {
            dashDirection = Vector3.zero;
            physicUtil.Timer.Stop();
        }

        public override void UpdateState()
        {
            physicUtil.AddForce(_player.transform, dashDirection, impulsePower * _player._stats.MoveSpeed, Time.deltaTime);

            if (physicUtil.Timer.IsFinished)
            {
                WhenDashEnds();
            }
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
            _player.TransitionToState(_player._stateFactory.GetState(StateFactory.StateType.Idle));
        }
    }
}
