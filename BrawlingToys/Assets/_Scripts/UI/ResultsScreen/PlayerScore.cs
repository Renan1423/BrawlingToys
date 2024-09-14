using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BrawlingToys.UI
{
    public class PlayerScore : MonoBehaviour
    {
        private int score;
        [SerializeField]
        private GameObject _scorePointPrefab;

        [Space(10)]

        [Header("References")]
        [SerializeField]
        private Transform _scorePointsHorizontalLayout;
        [SerializeField]
        private TextMeshProUGUI _playerIdText;
        [SerializeField]
        private Image _playerIcon;
        private List<ScorePoint> _scorePoints;

        public void FillPlayerScoreInfo(int requiredScoreToWin, int playerNumberId)
        {
            _scorePoints = new List<ScorePoint>();

            for (int i = 0; i < requiredScoreToWin; i++)
            {
                GameObject scorePointGo = Instantiate(_scorePointPrefab, _scorePointsHorizontalLayout);
                ScorePoint scorePoint = scorePointGo.GetComponent<ScorePoint>();
                scorePoint.ToggleScorePoint(false);
                _scorePoints.Add(scorePoint);
            }

            _playerIdText.text = "P" + (playerNumberId + 1);
        }

        public void AddScore(int scoreToAdd, ResultsScreen resultsScreen)
        {
            StartCoroutine(AddScoreCoroutine(scoreToAdd, resultsScreen));
        }

        private IEnumerator AddScoreCoroutine(int scoreToAdd, ResultsScreen resultsScreen)
        {
            int finalScore = score + scoreToAdd;
            if (finalScore > resultsScreen.GetRequiredScoreToWin())
                finalScore = resultsScreen.GetRequiredScoreToWin();

            while (score < finalScore)
            {
                _scorePoints[score].ToggleScorePoint(true);
                score++;

                yield return new WaitForSeconds(0.25f);
            }

            resultsScreen.UpdateScores();
        }


    }
}