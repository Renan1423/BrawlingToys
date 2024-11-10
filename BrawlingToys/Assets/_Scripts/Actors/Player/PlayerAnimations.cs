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
    [SerializeField] private AnimatorOverrideController controller;

    public UnityEvent OnAnimationAction;
    public UnityEvent OnAnimationEnd;

    private void Start() {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = controller;
    }

    // Da play na anima��o escolhida com base no AnimationType passado.
    public void PlayAnimation(AnimationType animationType)
    {
        switch (animationType)
        {
            case AnimationType.Idle:
                Play("Idle");
                break;
            case AnimationType.Movement:
                Play("Movement");
                break;
            case AnimationType.MeleeAttack:
                Play("MeleeAttack");
                break;
            case AnimationType.Dash:
                Play("Dash");
                break;
            case AnimationType.Die:
                Play("Die");
                break;
            default:
                break;
        }
    }

    public void Play(string stateName)
    {
        SetAnimatorStateServerRpc(stateName);
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

    [ServerRpc(RequireOwnership = false)]
    private void SetAnimatorStateServerRpc(string stateName)
    {
        SetAnimatorStateClientRpc(stateName); 
    }

    [ClientRpc]
    private void SetAnimatorStateClientRpc(string stateName)
    {
        Debug.Log(stateName);
        animator.Play(stateName);
    }
}
