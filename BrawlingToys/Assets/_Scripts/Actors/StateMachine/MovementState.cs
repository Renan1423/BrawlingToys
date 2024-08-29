using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementState : State
{
    public UnityEvent OnStep;

    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private float accel = 10f;

    private float currentSpeed;
    private float rotateSpeed = 10f;
    private Vector3 velocity;

    protected override void EnterState()
    {
        _player._animations.PlayAnimation(PlayerAnimations.AnimationType.Movement);
        _player._animations.OnAnimationAction.AddListener(() => OnStep?.Invoke());

        currentSpeed = 0;
        velocity = Vector3.zero;
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
        transform.position += velocity;
    }

    private void CalculateSpeed()
    {
        if(_player._inputs.GetMovementVectorNormalized().magnitude > 0)
        {
            currentSpeed += accel * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    private void CalculateVelocity()
    {
        Vector2 inputVector = _player._inputs.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        velocity = moveDirection * currentSpeed;

        _player.transform.forward = Vector3.Slerp(_player.transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
    }
}
