using UnityEngine;
using BrawlingToys.Network;
using System.Collections.Generic;
using BrawlingToys.Actors;
using BrawlingToys.Core;
using System;

namespace BrawlingToys.Managers
{
    public class MatchManager : NetworkSingleton<MatchManager>
    {
        private Dictionary<Player, PlayerRoundInfo> _playerMatchInfo;
        private int _deadPlayersCount = 0;

        protected override void Awake()
        {
            base.Awake();
            RestartMatchInfo();
        }

        private void RestartMatchInfo()
        {
            _deadPlayersCount = 0;

            if(_playerMatchInfo == null)
            {
                _playerMatchInfo = new Dictionary<Player, PlayerRoundInfo>();
                return;
            }

            foreach (Player player in _playerMatchInfo.Keys)
            {
                _playerMatchInfo[player] = new PlayerRoundInfo(0, true);
            }
        }

        public void AddPlayerMatchInfo(Player player)
        {
            _playerMatchInfo.Add(player, new PlayerRoundInfo(0, true));
        }

        public void RegisterKill(Player player)
        {
            _playerMatchInfo[player].KillsAmount++;
        }

        public void RegisterDeath(Player player)
        {
            _playerMatchInfo[player].IsSurvivor = false;

            _deadPlayersCount++;
            CheckMatchEnd();
        }

        private void CheckMatchEnd()
        {
            if(_playerMatchInfo.Count - _deadPlayersCount <= 1)
            {
                ScreenManager.instance.ToggleScreenByTag("ResultScreen", true);
            }
        }
    }
}
