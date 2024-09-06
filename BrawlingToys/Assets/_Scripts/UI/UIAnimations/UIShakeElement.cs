using DG.Tweening;
using UnityEngine;

namespace BrawlingToys.UI
{
    public class UIShakeElement : MonoBehaviour
    {
        private Sequence sequence;

        [SerializeField]
        private RectTransform uiElement;

        [Header("Shake Animation Settings")]
        public float animationTime = .5f;
        public float shakeStrength = 90;
        public float randomness = 90;
        public int vibrato = 90;
        public float delayBeteewnShakes = 3;
        public bool fadeOut = true;

        private void OnDestroy()
        {
            sequence.Kill();
            StopAllCoroutines();
        }

        private void Start()
        {
            PlayShake();
        }

        public void StopAnimation()
        {
            uiElement.rotation = Quaternion.identity;
            sequence.Kill();
        }

        public void PlayShake()
        {
            sequence.Kill();

            sequence = DOTween.Sequence()
                .Append(uiElement.DOShakeRotation(animationTime, shakeStrength, vibrato, randomness, fadeOut));
            sequence.SetLoops(-1, LoopType.Restart);
            sequence.AppendInterval(delayBeteewnShakes);
            sequence.Play();
        }
    }
}
