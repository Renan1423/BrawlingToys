using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimations : MonoBehaviour
{
    public enum AnimationType
    {
        Idle,
        Movement,
        MeleeAttack,
        Dash,
        Die
    }

    [SerializeField] private Animator animator;

    public UnityEvent OnAnimationAction;
    public UnityEvent OnAnimationEnd;

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

    public void Play(string name)
    {
        animator.Play(name, -1, 0f);
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
