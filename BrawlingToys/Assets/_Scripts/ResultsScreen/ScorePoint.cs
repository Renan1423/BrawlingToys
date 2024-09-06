using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePoint : MonoBehaviour
{
    [SerializeField]
    private GameObject _scorePointFill;

    public void ToggleScorePoint(bool active) 
    {
        _scorePointFill.SetActive(active);
    }
}
