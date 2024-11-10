using BrawlingToys.Core;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class PlayerCooldownController
    {
        private Player _player;

        public CountdownTimer meleeTimer;
        public CountdownTimer dashTimer;
        public CountdownTimer reloadTimer;
        public CountdownTimer fireRateTimer;

        public PlayerCooldownController(Player player)
        {
            _player = player;
        }

        public void Initialize()
        {
            _player.Stats.OnStatsChanged += Stats_OnStatsChanged;

            meleeTimer = new CountdownTimer(_player.Stats.MeleeCooldown);
            dashTimer = new CountdownTimer(_player.Stats.DashCooldown);
            reloadTimer = new CountdownTimer(_player.Stats.ReloadTime);
            fireRateTimer = new CountdownTimer(_player.Stats.FireRate);
        }

        private void Stats_OnStatsChanged(StatType statType)
        {
            switch (statType)
            {
                case StatType.MeleeCooldown:
                    meleeTimer.Reset(_player.Stats.MeleeCooldown);
                    break;
                case StatType.DashAmount:
                    dashTimer.Reset(_player.Stats.DashCooldown);
                    break;
                case StatType.ReloadTime:
                    reloadTimer.Reset(_player.Stats.ReloadTime);
                    break;
                case StatType.FireRate:
                    fireRateTimer.Reset(_player.Stats.FireRate);
                    break;
            }
        }

        public void UpdateCooldowns()
        {
            if (meleeTimer.IsRunning)
            {
                meleeTimer.Tick(Time.deltaTime);
            }

            if (dashTimer.IsRunning)
            {
                dashTimer.Tick(Time.deltaTime);
            }

            if (reloadTimer.IsRunning)
            {
                reloadTimer.Tick(Time.deltaTime);
            }

            if (fireRateTimer.IsRunning)
            {
                fireRateTimer.Tick(Time.deltaTime);
            }
        }

        private bool CompareValues(float currentValue, float newValue)
        {
            return currentValue != newValue;
        }
    }
}
