using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class PlayerNetworkAnimator : MonoBehaviour
    {
        [SerializeField] private NetworkAnimator _netAnim;
        [SerializeField] private Player _player; 

        private void Start()
        {
            if (!_player.IsOwner)
            {
                
            }
        } 
    }
}
