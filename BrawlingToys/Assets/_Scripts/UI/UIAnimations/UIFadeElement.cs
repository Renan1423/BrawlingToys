using DG.Tweening;
using UnityEngine;

namespace BrawlingToys.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIFadeElement : MonoBehaviour
    {
        private Sequence sequence;

        public event TweenCallback onCompleteSequence;

        [Header("Fade Animation Settings")]
        public float fadeTime = 1.0f;
        public float initAlpha = 0f, targetAlpha = 1f;
        public CanvasGroup canvasGroup;

        private void OnEnable()
        {
            onCompleteSequence += BlocksRaycasts;
            onCompleteSequence += KillSequence;
        }

        private void OnDisable()
        {
            onCompleteSequence -= BlocksRaycasts;
            onCompleteSequence -= KillSequence;

            canvasGroup.alpha = initAlpha;
            canvasGroup.blocksRaycasts = false;

            sequence.Kill();
        }

        private void KillSequence()
        {
            //Debug.Log("Sequence are killed");
            sequence.Kill();
        }

        public void PlayFade()
        {
            sequence.Kill();

            sequence = DOTween.Sequence();
            sequence.onComplete += onCompleteSequence;

            canvasGroup.blocksRaycasts = true;
            sequence
                .Append(canvasGroup.DOFade(targetAlpha, fadeTime))
                .Append(canvasGroup.DOFade(initAlpha, fadeTime));
            sequence.Play();
        }

        private void BlocksRaycasts()
        {
            canvasGroup.blocksRaycasts = false;
        }
    }
}
