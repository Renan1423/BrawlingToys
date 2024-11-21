using UnityEngine;
using UnityEngine.Events;

namespace BrawlingToys.Actors
{
    public class MovementState : State
    {
        public UnityEvent OnStep;

        [SerializeField] private MovementHandler _movementHandler;  
        
        [Space]
        
        [SerializeField] private float _accel, _deAccel;
        [SerializeField] private Transform _stepFeedback;

        private float _currentSpeed;
        private Vector3 _currentVelocity;
        private Vector3 _stepFeedbackOffset = new Vector3(0, 0, 0.1f);

        protected override void EnterState()
        {
            _player.Animations.PlayAnimation(PlayerAnimations.AnimationType.Movement);
            _player.Animations.OnAnimationAction.AddListener(() => OnStep?.Invoke());

            _currentSpeed = 0;
            _currentVelocity = Vector3.zero;
        }

        protected override void ExitState()
        {
            _player.Animations.ResetEvents();
        }

        public override void UpdateState()
        {
            CalculateVelocity();
            SetVelocity();
            
            if (_currentSpeed < .01f)
                _player.TransitionToState(_player.StateFactory.GetState(StateFactory.StateType.Idle));
            
            _stepFeedback.forward = (-1 * _currentVelocity) + _stepFeedbackOffset;
        }

        private void CalculateSpeed(Vector2 inputVector)
        {
            if (Mathf.Abs(inputVector.x) > 0 || Mathf.Abs(inputVector.y) > 0)
            {
                _currentSpeed += _accel * Time.deltaTime;
            }
            else
            {
                _currentSpeed -= _deAccel * Time.deltaTime;
            }
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _player.Stats.MoveSpeed);
        }

        private void CalculateVelocity()
        {
            CalculateSpeed(_player.Inputs.GetMovementVectorNormalized());

            Vector3 moveDirection = new Vector3(_player.Inputs.GetMovementVectorNormalized().x, 0, 
                _player.Inputs.GetMovementVectorNormalized().y);

            _currentVelocity = moveDirection * _currentSpeed;
            _currentVelocity.y = _player.Rb.velocity.y;
        }

        private void SetVelocity()
        {
            _movementHandler.ValidateWalk(transform.position); 
            _player.Rb.velocity = _currentVelocity; 
        }
    }
}
