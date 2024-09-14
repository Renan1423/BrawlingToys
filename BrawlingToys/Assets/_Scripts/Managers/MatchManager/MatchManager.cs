using BrawlingToys.Network;
using System.Collections.Generic;
using BrawlingToys.Actors;

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
                player.OnPlayerInitialize.AddListener(AddPlayerMatchInfo);
                player.OnPlayerKill.AddListener(RegisterKill);
                player.OnPlayerDeath.AddListener(RegisterDeath);
                _playerMatchInfo[player] = new PlayerRoundInfo(0, true);
            }
        }

        public void AddPlayerMatchInfo(Player player)
        {
            _playerMatchInfo.Add(player, new PlayerRoundInfo(0, true));
        }

        public void RegisterKill(Player player)
        {
            if (player == null)
                return;

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
                foreach (Player player in _playerMatchInfo.Keys)
                {
                    player.OnPlayerInitialize.RemoveListener(AddPlayerMatchInfo);
                    player.OnPlayerKill.RemoveListener(RegisterKill);
                    player.OnPlayerDeath.RemoveListener(RegisterDeath);
                }

                ScreenManager.instance.ToggleScreenByTag("ResultScreen", true);
            }
        }
    }
}
