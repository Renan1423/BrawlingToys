using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;
using Unity.Netcode;
using System.Linq;
using System.Collections;

namespace BrawlingToys.UI
{
    public class ResultsScreen : BaseScreen
    {
        [Header("Results Screen parameters")]
        [SerializeField]
        private int _requiredScoreToWin = 10;

        [Header("References")]
        [SerializeField]
        private GameObject _playerScorePrefab;
        [SerializeField]
        private Transform _scoreVerticalLayout;

        [Header("Settings")]

        [SerializeField] private float _timeToBackToGame = 7f; 

        private List<PlayerScore> _connectedPlayerScoresUIs;

        private Dictionary<ulong, PlayerRoundInfo> _playersRoundInfo = new(); 

        private bool _initialized = false;

        protected override void OnScreenEnable()
        {
            if(NetworkManager.Singleton.IsHost)
            {
                ServerGetPlayersRoundInfo();
                CallSyncRpc(); 
                DrawGraphics(); 
                StartCoroutine(WaitForCloseScreen());
            }
            

            void CallSyncRpc()
            {
                var serializedIds = _playersRoundInfo
                    .Keys
                    .ToArray(); 
                
                var serializedKills = _playersRoundInfo
                    .Values
                    .Select(i => i.KillsAmount)
                    .ToArray(); 
                
                var serializedSurvivals = _playersRoundInfo
                    .Values
                    .Select(i => i.IsSurvivor)
                    .ToArray();

                SyncPlayersInfoServerRpc(serializedIds, serializedKills, serializedSurvivals); 
            }
        }

        private void DrawGraphics()
        {
            if (!_initialized)
            {
                InitializeScreen();
                _initialized = true;
            }

            UpdateScores();
        }

        public void InitializeScreen()
        {
            _connectedPlayerScoresUIs = new List<PlayerScore>();
            int playerAmount = _playersRoundInfo.Count;

            for (int i = 0; i < playerAmount; i++)
            {
                GameObject playerScoreGo = Instantiate(_playerScorePrefab, _scoreVerticalLayout);
                PlayerScore playerScore = playerScoreGo.GetComponent<PlayerScore>();

                var playerIdKey = _playersRoundInfo.Keys.ToArray()[i]; 

                playerScore.FillPlayerScoreInfo(_requiredScoreToWin, (int) playerIdKey);
                _connectedPlayerScoresUIs.Add(playerScore);
            }
        }

        public void UpdateScores()
        {
            for (int i = 0; i < _playersRoundInfo.Count; i++)
            {
                var index = GetCastedIndex(i); 
                var roundInfo = _playersRoundInfo[index]; 
                
                var score = roundInfo.IsSurvivor 
                ? roundInfo.KillsAmount + 1 
                : roundInfo.KillsAmount;

                Debug.Log($"Index: {i} - Score {score}");

                _connectedPlayerScoresUIs[i].AddScore(score, this);
            }

            ulong GetCastedIndex(int i) => (ulong) i;  
        }

        public int GetRequiredScoreToWin() => _requiredScoreToWin;

        private IEnumerator WaitForCloseScreen()
        {
            yield return new WaitForSeconds(_timeToBackToGame); 

            CloseScreenServerRpc(); 
        }

        #region Network Actions

        private void ServerGetPlayersRoundInfo()
        {
            var matchInfo = MatchManager.LocalInstance.PlayerMatchInfo;  

            foreach (var key in matchInfo.Keys)
            {
                var player = key; 
                var info = matchInfo[key]; 

                var id = player.PlayerId; 

                if (_playersRoundInfo.ContainsKey(id))
                {
                    _playersRoundInfo[id] = info; 
                }
                else
                {
                    _playersRoundInfo.Add(id, info); 
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SyncPlayersInfoServerRpc(ulong[] playerIds, int[] killsAmount, bool[] isSurvivor)
        {
            SyncPlayersInfoClientRpc(playerIds, killsAmount, isSurvivor); 
        }

        [ClientRpc]
        private void SyncPlayersInfoClientRpc(ulong[] playerIds, int[] killsAmount, bool[] isSurvivor)
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                for (int i = 0; i < playerIds.Length; i++)
                {
                    var info = new PlayerRoundInfo(killsAmount[i], isSurvivor[i]); 
                    var id = playerIds[i]; 

                    if (_playersRoundInfo.ContainsKey(id))
                    {
                        _playersRoundInfo[id] = info; 
                    }
                    else
                    {
                        _playersRoundInfo.Add(id, info); 
                    }

                    Debug.Log($"{id}{info}");
                }
                
                DrawGraphics(); 
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CloseScreenServerRpc()
        {
            CloseScreenClientRpc(); 
        }

        [ClientRpc]
        private void CloseScreenClientRpc()
        {
            ScreenManager.instance.ToggleScreenByTag(ScreenName, false); 
        }

        #endregion
    }
}