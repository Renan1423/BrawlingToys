using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BrawlingToys.Managers;
using BrawlingToys.Actors;
using System.Linq;

namespace BrawlingToys.UI
{
    public abstract class PlayerCombatHud : NetworkBehaviour
    {
        private void Awake()
        {
            MatchManager.LocalInstance.OnPlayersSpawned += InitializePlayer;
        }

        private void InitializePlayer()
        {
            Player localPlayer = Player.Instances.First(p => p.IsOwner);
            ShowPlayerCombatHud(localPlayer);
        }

        public virtual void ShowPlayerCombatHud(Player player) 
        {
            if (player == null) 
            {
                Debug.LogError("PlayerCombatHud: Player is null!");
                return;
            }
        }
    }
}
