using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BrawlingToys.Managers;

namespace BrawlingToys.UI
{
    public abstract class PlayerCombatHud : NetworkBehaviour
    {
        protected virtual void OnEnable()
        {
            GameManager.LocalInstance.OnGameStateChange.AddListener(ShowPlayerCombatHud);
        }

        protected virtual void OnDisable()
        {
            GameManager.LocalInstance.OnGameStateChange.RemoveListener(ShowPlayerCombatHud);
        }

        public virtual void ShowPlayerCombatHud(GameStateType newGameState) 
        {
            return;
            if (newGameState != GameStateType.Combat)
                return;
        }
    }
}
