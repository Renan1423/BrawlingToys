using BrawlingToys.Actors;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimations : NetworkBehaviour
{

    public enum AnimationType
    {
        Idle,
        Movement,
        MeleeAttack,
        Dash,
        Die
    }

    private Animator animator;
    private AnimationHandler _animHandler; 

    [SerializeField] private AnimatorOverrideController controller;

    public UnityEvent OnAnimationAction;
    public UnityEvent OnAnimationEnd;

    private void Start() {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = controller;

        _animHandler = GetComponentInParent<AnimationHandler>(); 
    }

    // Da play na anima��o escolhida com base no AnimationType passado.
    public void PlayAnimation(AnimationType animationType)
    {
        switch (animationType)
        {
            case AnimationType.Idle:
                _animHandler.SetAnimatorStateServerRpc("Idle");
                break;
            case AnimationType.Movement:
                _animHandler.SetAnimatorStateServerRpc("Movement");
                break;
            case AnimationType.MeleeAttack:
                _animHandler.SetAnimatorStateServerRpc("MeleeAttack");
                break;
            case AnimationType.Dash:
                _animHandler.SetAnimatorStateServerRpc("Dash");
                break;
            case AnimationType.Die:
                _animHandler.SetAnimatorStateServerRpc("Die");
                break;
            default:
                break;
        }
    }

    public void Play(string stateName)
    {
        animator.Play(stateName); 
    }

    public void StartAnimation()
    {
        animator.enabled = true;
    }

    public void StopAnimation()
    {
        animator.enabled = false;
    }

    public void InvokeAnimationAction()
    {
        OnAnimationAction?.Invoke();
    }

    public void InvokeAnimationEnd()
    {
        OnAnimationEnd?.Invoke();
    }

    public void ResetEvents() //N�o remove callbacks adicionados no Inspector!!!!
    {
        OnAnimationAction.RemoveAllListeners();
        OnAnimationEnd.RemoveAllListeners();
    }
}
