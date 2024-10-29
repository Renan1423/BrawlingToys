using UnityEngine;

namespace BrawlingToys.Actors
{
    public class IdleState : State
    {
        protected override void EnterState()
        {
            _player.Animations.PlayAnimation(PlayerAnimations.AnimationType.Idle);
            _player.Inputs.GetMovementVectorNormalized();
        }

        protected override void HandleMovement(object sender, Vector2 inputVector)
        {
            if (Mathf.Abs(inputVector.x) > 0f || Mathf.Abs(inputVector.y) > 0f)
            {
                _player.TransitionToState(_player.StateFactory.GetState(StateFactory.StateType.Movement));
            }
        }
    }
}
