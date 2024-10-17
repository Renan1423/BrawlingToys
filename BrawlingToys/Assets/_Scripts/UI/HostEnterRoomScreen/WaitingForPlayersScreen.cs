using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrawlingToys.UI
{
    public class WaitingForPlayersScreen : BaseScreen
    {
        [Space(20)]

        [Header("Waiting For Players Screen")]
        [SerializeField]
        private List<WaitingPlayer> _waitingPlayerPanels;
        [SerializeField]
        private Button _playButton;

        private int _playersCount;

        protected override void OnScreenEnable()
        {
            RemoveAllPlayers();
        }

        public void AddPlayer(string playerName) 
        {
            _waitingPlayerPanels[_playersCount].AddPlayer(_playersCount, playerName);
            _playersCount++;
            ValidadeMatch();
        }

        public void RemovePlayer(int index) 
        {
            _waitingPlayerPanels[index].ResetPlayer();
            _playersCount--;

            //Turning, for example, the player 3 into the number 2 if the number 2 quit the match
            if (_waitingPlayerPanels[_playersCount + 1].HasPlayer) 
            {
                string playerName = _waitingPlayerPanels[_playersCount].PlayerName;

                _waitingPlayerPanels[_playersCount].AddPlayer(_playersCount, playerName);
                RemovePlayer(_playersCount + 1);
            }
        }

        private void RemoveAllPlayers() 
        {
            foreach (WaitingPlayer waitingPlayer in _waitingPlayerPanels)
            {
                waitingPlayer.ResetPlayer();
            }
        }

        private void ValidadeMatch() 
        {
            _playButton.interactable = (_playersCount > 1);
        }
    }
}
