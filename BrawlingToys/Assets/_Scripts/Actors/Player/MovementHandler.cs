using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class MovementHandler : NetworkBehaviour
    {
        [SerializeField] private Player _player; 
        [SerializeField] private Rigidbody _rig; 
        
        public void ValidateWalk(Vector3 actualPos)
        {
            var playerId = _player.PlayerId; 
            ValidateWalkServerRpc(playerId, actualPos); 
        }

        [ServerRpc]
        private void ValidateWalkServerRpc(ulong playerID, Vector3 actualPos)
        {
            var hostPlayerInstance = Player.Instances.First(p => p.PlayerId == playerID); 
            
            if (Vector3.Distance(hostPlayerInstance.transform.position, actualPos) > 1.5f)
            {
                
            }
        }
    }
}
