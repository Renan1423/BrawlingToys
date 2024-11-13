using BrawlingToys.Core;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class PlayerCooldownController
    {
        private Player _player;

        public CountdownTimer meleeTimer;
        public CountdownTimer dashTimer1;
        public CountdownTimer dashTimer2;
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
            dashTimer1 = new CountdownTimer(_player.Stats.DashCooldown);
            dashTimer2 = new CountdownTimer(_player.Stats.DashCooldown);
            reloadTimer = new CountdownTimer(_player.Stats.ReloadTime);
            fireRateTimer = new CountdownTimer(_player.Stats.FireRate);

            dashTimer1.OnTimerStart += () => _player.DashCount++;
            dashTimer1.OnTimerStop += () => _player.DashCount--;

            dashTimer2.OnTimerStart += () => _player.DashCount++;
            dashTimer2.OnTimerStop += () => _player.DashCount--;
        }

        private void Stats_OnStatsChanged(StatType statType)
        {
            switch (statType)
            {
                case StatType.MeleeCooldown:
                    meleeTimer.Reset(_player.Stats.MeleeCooldown);
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
            meleeTimer.Tick(Time.deltaTime);
            dashTimer1.Tick(Time.deltaTime);
            dashTimer2.Tick(Time.deltaTime);
            dashTimer2.Tick(Time.deltaTime);
            reloadTimer.Tick(Time.deltaTime);
            fireRateTimer.Tick(Time.deltaTime);
        }

        private bool CompareValues(float currentValue, float newValue)
        {
            return currentValue != newValue;
        }
    }
}
