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

        private List<PlayerScore> _playerScores;

        private Dictionary<ulong, PlayerRoundInfo> _playersRoundInfo = new(); 

        private ulong _updateIndex = 0;
        private bool _initialized = false;

        protected override void OnScreenEnable()
        {
            if(NetworkManager.Singleton.IsHost)
            {
                ServerGetPlayersRoundInfo();
                CallSyncRpc(); 
                InitializeResultScreenGraphics(); 
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

        private void InitializeResultScreenGraphics()
        {
            Debug.Log("Init result screen graphicsa");
            _updateIndex = 0;

            if (!_initialized)
            {
                //_requiredScoreToWin = Random.Range(6, 16);
                SpawnPlayerScoresObjects();
                _initialized = true;

                Debug.Log("Inited varible");
            }

            UpdateScores();
        }

        public void SpawnPlayerScoresObjects()
        {
            _playerScores = new List<PlayerScore>();

            //We must gather the amount of players using the multiplayer features. This is only for prototype purposes
            int playerAmount = _playersRoundInfo.Count;
            Debug.Log("Playert amoubnt" + playerAmount);

            for (int i = 0; i < playerAmount; i++)
            {
                GameObject playerScoreGo = Instantiate(_playerScorePrefab, _scoreVerticalLayout);
                PlayerScore playerScore = playerScoreGo.GetComponent<PlayerScore>();

                var playerIdKey = _playersRoundInfo.Keys.ToArray()[i]; 

                playerScore.FillPlayerScoreInfo(_requiredScoreToWin, (int) playerIdKey);
                _playerScores.Add(playerScore);
            }
        }

        public void UpdateScores()
        {
            if ((int) _updateIndex >= _playerScores.Count)
                return;

            Debug.Log("Updating Scrioe");

            int additionalScore = _playersRoundInfo[_updateIndex].IsSurvivor 
            ? _playersRoundInfo[_updateIndex].KillsAmount + 1 
            : _playersRoundInfo[_updateIndex].KillsAmount;

            _updateIndex++;

            _playerScores[(int) _updateIndex - 1].AddScore(additionalScore, this);
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
                
                InitializeResultScreenGraphics(); 
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