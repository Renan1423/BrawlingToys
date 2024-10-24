using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Network
{
    public class RpcCallsDebug : NetworkBehaviour
    {
        [ContextMenu("Debug message in all clients with ServerRpc")]
        private void SendMessageCall()
        {
            SendMessageServerRpc(); 
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendMessageServerRpc()
        {
            SendMessageClientRpc(); 
        }

        [ClientRpc]
        private void SendMessageClientRpc()
        {
            Debug.Log("Client RPC debug");
        }
    }
}
