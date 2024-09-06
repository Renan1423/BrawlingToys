using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResultsScreen : MonoBehaviour
{
    [Header("Results Screen parameters")]
    [SerializeField]
    private int _requiredScoreToWin = 10;

    [Header("References")]
    [SerializeField]
    private GameObject _playerScorePrefab;
    [SerializeField]
    private Transform _scoreVerticalLayout;
    [SerializeField]
    //Only for prototype purposes. It must be deleted later
    private AssetReference _playerAssetReference;

    private List<PlayerScore> _playerScores;
    private List<PlayerRoundInfo> _playersRoundInfo;

    private int _updateIndex = 0;
    private bool _initialized = false;

    private void OnEnable()
    {
        GatherPlayerInformations();
        _updateIndex = 0;

        if (!_initialized)
        {
            _requiredScoreToWin = Random.Range(6, 16);
            SpawnPlayerScoresObjects();
            _initialized = true;
        }

        UpdateScores();
    }

    public void GatherPlayerInformations()
    {
        _playersRoundInfo = new List<PlayerRoundInfo>();

        //We must gather the players informations using the multiplayer features. This is only for prototype purposes
        for (int i = 0; i < 6; i++)
        {
            int randomKillsAmount = Random.Range(0, 7);
            PlayerRoundInfo playerRoundInfo = new PlayerRoundInfo(i, _playerAssetReference, randomKillsAmount, true);
            _playersRoundInfo.Add(playerRoundInfo);
        }
    }

    public void SpawnPlayerScoresObjects()
    {
        _playerScores = new List<PlayerScore>();

        //We must gather the amount of players using the multiplayer features. This is only for prototype purposes
        int playerAmount = Random.Range(2, 7);

        for (int i = 0; i < playerAmount; i++)
        {
            GameObject playerScoreGo = Instantiate(_playerScorePrefab, _scoreVerticalLayout);
            PlayerScore playerScore = playerScoreGo.GetComponent<PlayerScore>();

            playerScore.FillPlayerScoreInfo(_playersRoundInfo[i].PlayerNumber, _playersRoundInfo[i].PlayerCharacter, _requiredScoreToWin);
            _playerScores.Add(playerScore);
        }

    }

    public void UpdateScores()
    {
        if (_updateIndex >= _playerScores.Count)
            return;

        int additionalScore = (_playersRoundInfo[_updateIndex].IsSurvivor) ? _playersRoundInfo[_updateIndex].KillsAmount + 1 :
             _playersRoundInfo[_updateIndex].KillsAmount;

        _updateIndex++;

        _playerScores[_updateIndex - 1].AddScore(additionalScore, this);
    }

    public int GetRequiredScoreToWin() => _requiredScoreToWin;
}
