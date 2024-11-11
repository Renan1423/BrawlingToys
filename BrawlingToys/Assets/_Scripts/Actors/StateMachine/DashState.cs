using BrawlingToys.Core;
using System;
using System.Threading;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class DashState : State
    {
        [SerializeField] private float _impulsePower = 20.0f;
        [SerializeField] private float _dashDistance = 2.0f;

        [SerializeField] private float rayYOffset = .5f;
        [SerializeField] private LayerMask _obstacleLayers;

        [SerializeField] private Transform _dashFeedback;

        private float _finalDashDistance = 0f;
        private float _traveledDistance = 0f;
        private Vector3 _dashDirection;
        private RaycastHit hitInfo;

        protected override void EnterState()
        {
            _player.Animations.PlayAnimation(PlayerAnimations.AnimationType.Dash);

            // Aplicar for�a no player na dire��o de movimento
            float movementMagnitude = _player.Inputs.GetMovementVectorNormalized().sqrMagnitude;
            Vector3 movementDirection = new Vector3(_player.Inputs.GetMovementVectorNormalized().x, 0,
                _player.Inputs.GetMovementVectorNormalized().y);

            _dashDirection = movementMagnitude > 0 ? movementDirection : _player.transform.forward;

            _finalDashDistance = _dashDistance;

            Physics.Raycast(new Vector3(_player.transform.position.x, _player.transform.position.y + rayYOffset, _player.transform.position.z),
                _dashDirection, out hitInfo, _dashDistance, _obstacleLayers);
            if(hitInfo.collider != null )
            {
                _finalDashDistance = Vector3.Distance(_player.transform.position, hitInfo.point);
            }

            _dashFeedback.forward = _dashDirection;
        }

        protected override void ExitState()
        {
            _dashDirection = Vector3.zero;
            _player.Rb.velocity = Vector3.zero;
            _traveledDistance = 0f;
            _player.Animations.ResetEvents();
        }

        public override void UpdateState()
        {
            if (_traveledDistance <= _finalDashDistance)
            {
                _traveledDistance += _impulsePower * Time.deltaTime;
                _player.Rb.velocity = _impulsePower * _dashDirection;
            }
            else
            {
                _traveledDistance = 0f;
                _player.TransitionToState(_player.StateFactory.GetState(StateFactory.StateType.Idle));
            }
        }

        public override void FixedUpdateState()
        {
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
    }
}
