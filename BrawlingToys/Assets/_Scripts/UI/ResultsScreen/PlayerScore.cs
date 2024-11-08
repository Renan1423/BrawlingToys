using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using MoreMountains.Feedbacks;

namespace BrawlingToys.UI
{
    public class PlayerScore : MonoBehaviour
    {
        private int score;
        private ulong playerId; 
        
        [SerializeField]
        private GameObject _scorePointPrefab;

        [Space(10)]

        [Header("References")]
        [SerializeField]
        private Transform _scorePointsHorizontalLayout;
        [SerializeField]
        private TextMeshProUGUI _playerIdText;
        [SerializeField]
        private Image _playerIdImage;
        [SerializeField]
        private Image _playerIcon;
        [SerializeField]
        private Image _scorePanel;
        private List<ScorePoint> _scorePoints;
        [SerializeField]
        private MMF_Player _feedbacks;

        public int Score { get => score; }
        public ulong PlayerId { get => playerId; }

        public void FillPlayerScoreInfo(int requiredScoreToWin, int playerNumberId, PlayerScoreColorPallete playerColorPallete)
        {
            _scorePoints = new List<ScorePoint>();
            _scorePanel.color = playerColorPallete.BaseColor;

            for (int i = 0; i < requiredScoreToWin; i++)
            {
                GameObject scorePointGo = Instantiate(_scorePointPrefab, _scorePointsHorizontalLayout);
                ScorePoint scorePoint = scorePointGo.GetComponent<ScorePoint>();
                scorePoint.ToggleScorePoint(false);
                scorePoint.FilledImage.color = playerColorPallete.LightColor;
                scorePoint._UnfilledImage.color = playerColorPallete.DarkColor;
                _scorePoints.Add(scorePoint);
            }

            playerId = (ulong) playerNumberId; 
            
            _playerIdText.text = "P" + (playerNumberId + 1);
            _playerIdImage.color = playerColorPallete.BaseColor;
        }


        public IEnumerator AddScoreCoroutine(int scoreToAdd, ResultsScreen resultsScreen)
        {
            int finalScore = score + scoreToAdd;
            if (finalScore > resultsScreen.GetRequiredScoreToWin())
                finalScore = resultsScreen.GetRequiredScoreToWin();

            while (score < finalScore)
            {
                _scorePoints[score].ToggleScorePoint(true);
                score++;
                _feedbacks.PlayFeedbacks();

                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}