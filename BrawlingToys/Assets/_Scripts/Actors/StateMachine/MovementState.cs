using UnityEngine;
using UnityEngine.Events;

namespace BrawlingToys.Actors
{
    public class MovementState : State
    {
        public UnityEvent OnStep;

        [SerializeField] private Rigidbody _rig; 
        [SerializeField] private float _moveSpeed; 

        [Space]

        [SerializeField] private float _accel = 10f;

        private float _currentSpeed;
        private Vector3 _velocity;

        protected override void EnterState()
        {
            _player._animations.PlayAnimation(PlayerAnimations.AnimationType.Movement);
            _player._animations.OnAnimationAction.AddListener(() => OnStep?.Invoke());

            _currentSpeed = 0;
            _velocity = Vector3.zero;
        }

        protected override void ExitState()
        {
            _player._animations.ResetEvents();
        }

        public override void UpdateState()
        {
            CalculateSpeed();
            CalculateVelocity();

            if (_player._inputs.GetMovementVectorNormalized() == Vector2.zero)
            {
                _player.TransitionToState(_player._stateFactory.GetState(StateFactory.StateType.Idle));
            }
        }

        public override void FixedUpdateState()
        {
            _rig.MovePosition(_rig.position + _velocity * _moveSpeed * Time.fixedDeltaTime);
        }

        private void CalculateSpeed()
        {
            if (_player._inputs.GetMovementVectorNormalized().magnitude > 0)
            {
                _currentSpeed += _accel * Time.deltaTime;
            }

            _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _player._stats.MoveSpeed);
        }

        private void CalculateVelocity()
        {
            Vector2 inputVector = _player._inputs.GetMovementVectorNormalized();
            Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

            _velocity = moveDirection * _currentSpeed;
        }
    }
}
