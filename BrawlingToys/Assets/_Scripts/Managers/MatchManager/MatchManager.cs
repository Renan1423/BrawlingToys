using System.Collections.Generic;
using System.Linq;
using BrawlingToys.Actors;
using BrawlingToys.Network;
using Unity.Netcode;

namespace BrawlingToys.Managers
{
    public class MatchManager : NetworkSingleton<MatchManager>
    {
        private Dictionary<Player, PlayerRoundInfo> _playerMatchInfo;
        private int _deadPlayersCount = 0;

        public Player[] MatchPlayers { get {
            return _playerMatchInfo.Keys.ToArray();  
        } }

        protected override void Awake()
        {
            base.Awake(); 
            if(IsHost) RestartMatchInfo();
        }

        private void RestartMatchInfo()
        {
            _deadPlayersCount = 0;

            if(_playerMatchInfo == null)
            {
                _playerMatchInfo = new Dictionary<Player, PlayerRoundInfo>();
                //return;
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
            if(MatchIsEnded())
            {
                foreach (Player player in _playerMatchInfo.Keys)
                {
                    player.OnPlayerInitialize.RemoveListener(AddPlayerMatchInfo);
                    player.OnPlayerKill.RemoveListener(RegisterKill);
                    player.OnPlayerDeath.RemoveListener(RegisterDeath);
                }

                CallResultScreenServerRpc(); 
            }

            bool MatchIsEnded() => _playerMatchInfo.Count - _deadPlayersCount <= 1; 
        }

        [ServerRpc(RequireOwnership = false)]
        private void CallResultScreenServerRpc()
        {
            CallResultScreenClientRpc(); 
        }

        [ClientRpc]
        private void CallResultScreenClientRpc()
        {
            ScreenManager.instance.ToggleScreenByTag("ResultScreen", true);
        } 
    }
}
