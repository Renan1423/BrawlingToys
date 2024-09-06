using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace BrawlingToys.UI
{
    public class UIScaleElement : MonoBehaviour
    {
        private Sequence sequence;

        [SerializeField]
        private RectTransform uiElement;

        [Header("Scale Animation Settings")]
        public float animantionEndScale;
        public float animationTime;
        public bool playConstantly = false;

        private Vector3 baseScale, endScale;

        private void OnDestroy()
        {
            sequence.Kill();
            StopAllCoroutines();
        }

        private void Start()
        {
            baseScale = uiElement.localScale;
            endScale = Vector3.one * animantionEndScale;

            if (playConstantly)
            {
                PlayScaleConstatly();
            }
        }

        public void StopAnimation()
        {
            uiElement.localScale = baseScale;
            sequence.Kill();
        }

        public void PlayScaleConstatly()
        {
            sequence.Kill();
            sequence = DOTween.Sequence()
                    .Append(uiElement.DOScale(baseScale, animationTime))
                    .Append(uiElement.DOScale(endScale, animationTime));
            sequence.SetLoops(-1, LoopType.Yoyo);
            sequence.Play();
        }

        public void PlayScale(bool val)
        {
            StopAllCoroutines();

            if (val)
            {
                DOTween.Kill(sequence);
                sequence = DOTween.Sequence().Append(uiElement.DOScale(endScale, animationTime));
                sequence.Play();
            }
            else
            {
                DOTween.Kill(sequence);
                sequence = DOTween.Sequence().Append(uiElement.DOScale(Vector3.zero, animationTime));
                sequence.Play();
            }

            StartCoroutine(TimeToKillSequence());
        }

        IEnumerator TimeToKillSequence()
        {
            yield return new WaitForSeconds(animationTime + .1f);
            DOTween.Kill(sequence);
        }
    }
}
