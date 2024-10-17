using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BrawlingToys.UI
{
    public class WaitingPlayer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _playerNumberText;
        [SerializeField]
        private TextMeshProUGUI _playerNameText;

        [Space(10)]

        [SerializeField]
        private Image _panel;
        [SerializeField]
        private Color _panelColor = Color.white;
        [SerializeField]
        private Color _initialPanelColor = Color.white;

        public bool HasPlayer { get; private set; }
        public string PlayerName => _playerNameText.text;

        public void AddPlayer(int index, string playerName) 
        {
            _playerNumberText.text = "P" + index;
            _playerNameText.text = playerName;
            _panel.color = _panelColor;
        }

        public void ResetPlayer() 
        {
            _playerNameText.text = "";
            _playerNumberText.text = "?";
            _panel.color = _initialPanelColor;
        }
    }
}
