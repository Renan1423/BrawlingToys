using System.Threading.Tasks;
using BrawlingToys.DesignPatterns;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.Events;

namespace BrawlingToys.Network
{
    public class RelayParty : ContextSingleton<RelayParty>
    {        
        [HideInInspector] public UnityEvent OnNewPlayerConnected = new(); 
        
        [Header("Lobby Settings")]
        [SerializeField] private int _lobbySize = 6; 

        [Header("Logs")]

        [SerializeField] private bool _enableLogs; 
        
        /// <summary>
        /// Create the party to other player can join. The player who create the party will be the host. 
        /// </summary>
        /// <returns>
        /// A tuple with the operation success status and if the operation work out return the party code
        /// </returns>
        public async Task<(bool success, string partyCode)> CreatePartyAsync()
        {
            try 
            {
                var alloc = await RelayService.Instance.CreateAllocationAsync(_lobbySize);

                var joinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId); 
                Debug.Log(joinCode);

                NetworkManager.Singleton.GetComponent<UnityTransport>()
                .SetHostRelayData(
                    alloc.RelayServer.IpV4,
                    (ushort) alloc.RelayServer.Port,
                    alloc.AllocationIdBytes,
                    alloc.Key,
                    alloc.ConnectionData
                ); 

                NetworkManager.Singleton.StartHost(); 

                OnNewPlayerConnected?.Invoke();

                return (true, joinCode); 
            }
            catch (RelayServiceException e) 
            {
                Debug.Log("Error on party creation, more details: " + e);
                
                return (false, string.Empty); 
            }
        }

        /// <summary>
        /// Try to connect in other party as client
        /// </summary>
        /// <param name="lobbyCode">
        /// The party code you want to connect to
        /// </param>
        /// <returns>
        /// If the operation work out
        /// </returns>
        public async Task<bool> TryJoinPartyAsync(string lobbyCode)
        {
            try 
            {
                var joinAlloc = await RelayService.Instance.JoinAllocationAsync(lobbyCode);  

                NetworkManager.Singleton.GetComponent<UnityTransport>()
                    .SetClientRelayData(
                        joinAlloc.RelayServer.IpV4,
                        (ushort) joinAlloc.RelayServer.Port,
                        joinAlloc.AllocationIdBytes,
                        joinAlloc.Key,
                        joinAlloc.ConnectionData,
                        joinAlloc.HostConnectionData
                    );

                NetworkManager.Singleton.StartClient(); 

                OnNewPlayerConnected?.Invoke(); 

                Debug.Log($"Joined!");

                return true; 
            }
            catch (RelayServiceException e) 
            {
                Debug.Log(e);
                return false; 
            }
        }
    }
}
