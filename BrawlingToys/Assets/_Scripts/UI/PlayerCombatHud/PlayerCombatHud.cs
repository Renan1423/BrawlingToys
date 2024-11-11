using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BrawlingToys.Managers;
using BrawlingToys.Actors;

namespace BrawlingToys.UI
{
    public abstract class PlayerCombatHud : NetworkBehaviour
    {

        public virtual void ShowPlayerCombatHud(GameStateType newGameState, Player player) 
        {
            if (player == null) 
            {
                Debug.LogError("PlayerCombatHud: Player is null!");
                return;
            }
        }
    }
}
