using DG.Tweening;
using UnityEngine;

public class PickableModifier : MonoBehaviour
{
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Transform shadowTransform;

    [Header("DOTween Parameters")]
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease ease;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
        PlaySequence();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void PlaySequence()
    {
        visualTransform
            .DOMoveY(visualTransform.position.y + moveDistance, moveDuration)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
