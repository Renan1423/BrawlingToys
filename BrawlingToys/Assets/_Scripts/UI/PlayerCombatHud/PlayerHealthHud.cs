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

        protected override void OnEnable()
        {
            base.OnEnable();

            Player player = NetworkManager.LocalClient.PlayerObject.GetComponent<Player>();
            player.OnTakeDamage += UpdateHealthHud;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Player player = NetworkManager.LocalClient.PlayerObject.GetComponent<Player>();
            player.OnTakeDamage -= UpdateHealthHud;
        }

        public override void ShowPlayerCombatHud(GameStateType newGameState)
        {
            base.ShowPlayerCombatHud(newGameState);

            IDamageable playerDamageable = NetworkManager.LocalClient.PlayerObject.GetComponent<IDamageable>();

            ShowHealthHud(playerDamageable.Health);
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
