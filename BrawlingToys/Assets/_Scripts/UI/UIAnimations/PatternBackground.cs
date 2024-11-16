using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BrawlingToys.UI
{
    public class PatternBackground : MonoBehaviour
    {
        [SerializeField]
        private Transform patternTrans;
        [SerializeField]
        private Vector2 initPos = new Vector2(1926f, -1001f);
        [SerializeField]
        private Vector2 endPos = new Vector2(-1926f, 1001f);
        [SerializeField]
        private float animationDuration = 10f;
        private bool goingForwardMovement;

        private void Start()
        {
            ToggleMovement();
        }

        private void ToggleMovement() 
        {
            goingForwardMovement = !goingForwardMovement;

            Vector2 targetPos = (goingForwardMovement) ? endPos : initPos;

            patternTrans.DOLocalMove(targetPos, animationDuration);
            Invoke(nameof(ToggleMovement), animationDuration);
        }
    }
}
