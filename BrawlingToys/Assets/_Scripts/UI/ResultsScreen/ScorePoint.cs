using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

namespace BrawlingToys.UI
{
    public class ScorePoint : MonoBehaviour
    {
        [SerializeField]
        private GameObject _scorePointFill;
        [field: SerializeField]
        public Image FilledImage { get; private set; }
        [field: SerializeField]
        public Image _UnfilledImage { get; private set; }
        [SerializeField]
        private MMF_Player _feedbacks;

        public void ToggleScorePoint(bool active)
        {
            _scorePointFill.SetActive(active);
            _feedbacks.PlayFeedbacks();
        }
    }
}