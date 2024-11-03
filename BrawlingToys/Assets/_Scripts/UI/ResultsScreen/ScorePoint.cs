using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BrawlingToys.UI
{
    public class ScorePoint : MonoBehaviour
    {
        [SerializeField]
        private GameObject _scorePointFill;
        [SerializeField]
        private UnityEvent _OnToggleScore;
        [field: SerializeField]
        public Image FilledImage { get; private set; }
        [field: SerializeField]
        public Image _UnfilledImage { get; private set; }

        public void ToggleScorePoint(bool active)
        {
            _scorePointFill.SetActive(active);
            _OnToggleScore?.Invoke();
        }
    }
}