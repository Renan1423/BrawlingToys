using BrawlingToys.Actors;
using BrawlingToys.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.UI
{
    public class PlayerBuffsHud : PlayerCombatHud
    {
        [SerializeField]
        private Transform _buffsHorizontalLayout;

        public override void ShowPlayerCombatHud(GameStateType newGameState)
        {
            base.ShowPlayerCombatHud(newGameState);

            if (NetworkManager.LocalClient.PlayerObject == null)
                return;

            Player player = NetworkManager.LocalClient.PlayerObject.GetComponent<Player>();

            if (player.Stats.Mediator.GetAppliedModifiers() == null)
                return;

            foreach (ModifierScriptable mod in player.Stats.Mediator.GetAppliedModifiers())
            {
                EffectIconGenerator.instance.CreateEffectIcon(mod, _buffsHorizontalLayout);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H)) 
            {
                ShowPlayerCombatHud(GameStateType.Combat);
            }
        }

        private void ResetBuffsHud() 
        {
            int spawnedBuffsIcons = _buffsHorizontalLayout.childCount;
            for (int i = 0; i < spawnedBuffsIcons; i++)
            {
                Destroy(_buffsHorizontalLayout.GetChild(i).gameObject);
            }
        }
    }
}
