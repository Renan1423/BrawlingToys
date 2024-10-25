using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Network;
using BrawlingToys.Actors;
using TMPro;


namespace BrawlingToys.UI
{
    public class ClientWaitScreen : BaseScreen
    {
        [Space(20)]
        [Header("References")]
        [SerializeField]
        private JoinRoom _joinRoom;

        [Space(20)]
        [Header("Client Waiting Room")]
        [SerializeField]
        private TextMeshProUGUI _playersAmountText;
        private int _playersAmount = 1;

        private void OnEnable()
        {
            _joinRoom.OnNewPlayerJoined += OnNewPlayerJoined;
        }

        private void OnDisable()
        {
            _joinRoom.OnNewPlayerJoined -= OnNewPlayerJoined;
        }

        public void OnNewPlayerJoined(PlayerClientData playerClientData) 
        {
            _playersAmount = FindObjectsOfType<PlayerClientData>().Length;
            _playersAmountText.text = "Jogadores na partida: " + _playersAmount + "/4";
        }
    }
}
