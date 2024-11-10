using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;
using Unity.Netcode;
using System.Linq;
using System.Collections;

namespace BrawlingToys.UI
{
    [System.Serializable]
    public struct PlayerScoreColorPallete 
    {
        public Color BaseColor;
        public Color DarkColor;
        public Color LightColor;
    }

    public class ResultsScreen : BaseScreen
    {
        [Header("Results Screen parameters")]
        [SerializeField]
        private PlayerScoreColorPallete[] _playersColors;

        [Header("References")]
        [SerializeField]
        private GameObject _playerScorePrefab;
        [SerializeField]
        private Transform _scoreVerticalLayout;

        [Header("Settings")]

        private List<PlayerScore> _connectedPlayerScoresUIs;

        private Dictionary<ulong, PlayerRoundInfo> _playersRoundInfo = new(); 

        private bool _initialized = false;
        private bool _updatedScore = false;

        protected override void OnScreenEnable()
        {
            if(NetworkManager.Singleton.IsHost)
            {
                _updatedScore = false;

                ServerGetPlayersRoundInfo();
                CallSyncRpc(); 
                DrawGraphics(); 
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

           StartCoroutine(UpdateScores());
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

                int requiredScoreToWin = GetRequiredScoreToWin();

                playerScore.FillPlayerScoreInfo(requiredScoreToWin, (int) playerIdKey, _playersColors[i]);
                _connectedPlayerScoresUIs.Add(playerScore);
            }
        }

        public IEnumerator UpdateScores()
        {
            for (int i = 0; i < _playersRoundInfo.Count; i++)
            {
                yield return UpdatePlayerScore(i);
            }

            yield return new WaitForSeconds(2);

            if(!MatchIsEnded())
            {
                ChangeToNextScreenServerRpc(); 
            }
            else
            {
                if (LocalPlayerIsWinner())
                {
                    LevelManager.LocalInstance.LocalLoadSceneByName("WinnerScene"); 
                }
                else
                {
                    LevelManager.LocalInstance.LocalLoadSceneByName("LoserScene");
                }
            }

            bool MatchIsEnded() => _connectedPlayerScoresUIs.Any(ui => ui.Score >= GetRequiredScoreToWin()); 

            bool LocalPlayerIsWinner() => _connectedPlayerScoresUIs
                .First(ui => ui.PlayerId == NetworkManager.Singleton.LocalClientId)
                .Score >= GetRequiredScoreToWin(); 
        }

        private IEnumerator UpdatePlayerScore(int playerIndex) 
        {
            var index = GetCastedIndex(playerIndex);
            var roundInfo = _playersRoundInfo[index];

            var score = roundInfo.IsSurvivor
            ? roundInfo.KillsAmount + 1
            : roundInfo.KillsAmount;

            Debug.Log($"Index: {playerIndex} - Score {score}");

            yield return _connectedPlayerScoresUIs[playerIndex].AddScoreCoroutine(score, this);

            ulong GetCastedIndex(int i) => (ulong)i;
        }

        //public int GetRequiredScoreToWin() => PlayerClientDatasManager.LocalInstance.PlayerClientDatas[0].RequiredPointsToWin;
        public int GetRequiredScoreToWin() => 10;

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
        private void ChangeToNextScreenServerRpc()
        {
            ChangeToNextScreenClientRpc(); 
        }

        [ClientRpc]
        private void ChangeToNextScreenClientRpc()
        {
            ScreenManager.instance.ToggleScreenByTag(ScreenName, false); 
            ScreenManager.instance.ToggleScreenByTag("SupriseBox", true); 
        }

        #endregion
    }
}