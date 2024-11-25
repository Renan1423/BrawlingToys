using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class AnimationHandler : NetworkBehaviour
    {
        [SerializeField] private Player _player; 
        
        [ServerRpc(RequireOwnership = false)]
        public void SetAnimatorStateServerRpc(string stateName)
        {
            SetAnimatorStateClientRpc(stateName); 
        }

        [ClientRpc]
        private void SetAnimatorStateClientRpc(string stateName)
        {
            Debug.Log(stateName);
            _player.Animations.Play(stateName);
        }
    }
}
