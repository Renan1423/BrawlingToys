using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.UI
{
    public class Tutorial : BaseScreen
    {
        [Header("Tutorial")]

        [SerializeField] private GameObject _button; 
        [SerializeField] private GameObject _waitingMsm; 

        private int SERVER_readyPlayers; 
        
        private bool _isClosing;

        public void CloseTutorial() 
        {
            _button.SetActive(false); 
            _waitingMsm.SetActive(true); 

            OnClientReadyServerRpc(); 
        }

        [ServerRpc(RequireOwnership = false)]
        private void OnClientReadyServerRpc()
        {
            SERVER_readyPlayers++; 
            
            var totalConnectedClients = NetworkManager
                .Singleton.ConnectedClients.Count;  

            if (SERVER_readyPlayers >= totalConnectedClients)
            {
                CloseScreenClientRpc(); 
            }
        }

        [ClientRpc]
        private void CloseScreenClientRpc()
        {
            CloseScreen(0.25f);
        }
    }
}
