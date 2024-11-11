using BrawlingToys.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Actors;

namespace BrawlingToys.UI
{
    public class PlayerHealthHud : PlayerCombatHud
    {
        [SerializeField]
        private GameObject _hpPrefab;
        [SerializeField]
        private Transform _hpHorizontalLayout;
        private List<HealthPoint> _hpsList;

        public override void ShowPlayerCombatHud(GameStateType newGameState, Player player)
        {
            base.ShowPlayerCombatHud(newGameState, player);

            PlayerHit playerHit = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerHit>();

            if(playerHit != null)
                ShowHealthHud(playerHit.MaxLife);

            playerHit.OnPlayerLifeChange.AddListener(UpdateHealthHud);
        }

        private void ShowHealthHud(int playerHealth)
        {
            ResetHealthHud();

            for (int i = 0; i < playerHealth; i++)
            {
                GameObject hpGo = Instantiate(_hpPrefab, _hpHorizontalLayout);
                HealthPoint hp = hpGo.GetComponent<HealthPoint>();
                _hpsList.Add(hp);
            }
        }

        private void UpdateHealthHud(int currentHealth) 
        {
            for (int i = 0; i < _hpsList.Count; i++)
            {
                if (currentHealth < i)
                    _hpsList[i].DisableHealthPoint();
            }
        }

        private void ResetHealthHud() 
        {
            int spawnedHPsCount = _hpHorizontalLayout.childCount;
            for (int i = 0; i < spawnedHPsCount; i++)
            {
                Destroy(_hpHorizontalLayout.GetChild(i).gameObject);
            }

            _hpsList = new List<HealthPoint>();
        }
    }
}
