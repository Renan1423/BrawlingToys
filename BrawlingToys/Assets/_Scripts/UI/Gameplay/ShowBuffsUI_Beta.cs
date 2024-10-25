using System;
using System.Collections.Generic;
using System.Linq;
using BrawlingToys.Actors;
using BrawlingToys.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrawlingToys.UI
{
    public class ShowBuffsUI_Beta : MonoBehaviour
    {
        [Header("References")]

        [SerializeField] private TMP_Text _buffText; 
        
        private Player _localPlayer;

        private void Awake()
        {
            MatchManager.LocalInstance.OnPlayersSpawned += InitializePlayer; 
        }

        private void InitializePlayer()
        {
            _localPlayer = Player.Instances.First(p => p.IsOwner); 
            _localPlayer.Stats.Mediator.OnMediatorChange += UpdateBuffDebuffText; 
        }

        private void UpdateBuffDebuffText(object sender, List<ModifierScriptable> appliedBuffs)
        {
            var baseText = "Buffs: {buff01} - {buff02} - {buff03}";
            
            var finalText = baseText
                .Replace("{buff01}", appliedBuffs.Count >= 1 ? appliedBuffs[0].ToString() : "N/D")  
                .Replace("{buff02}", appliedBuffs.Count >= 2 ? appliedBuffs[1].ToString() : "N/D")
                .Replace("{buff03}", appliedBuffs.Count == 3 ? appliedBuffs[2].ToString() : "N/D");

            _buffText.SetText(finalText);   
        }
    }
}
