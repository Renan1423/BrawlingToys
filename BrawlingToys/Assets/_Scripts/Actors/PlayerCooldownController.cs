using BrawlingToys.Core;
using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class PlayerCooldownController
    {
        public static Action<StatType, float> OnSomeCooldownChange;

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
            OnSomeCooldownChange = CooldownChange;

            meleeTimer = new CountdownTimer(_player._stats.MeleeCooldown);
            dashTimer = new CountdownTimer(_player._stats.DashCooldown);
            reloadTimer = new CountdownTimer(_player._stats.ReloadTime);
            fireRateTimer = new CountdownTimer(_player._stats.FireRate);
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

        private void CooldownChange(StatType statType, float value)
        {
            switch (statType)
            {
                case StatType.MeleeCooldown:
                    if (CompareValues(_player._stats.MeleeCooldown, value))
                    {
                        meleeTimer.Reset(value);
                    }
                    break;
                case StatType.DashCooldown:
                    if (CompareValues(_player._stats.DashCooldown, value))
                    {
                        dashTimer.Reset(value);
                    }
                    break;
                case StatType.ReloadTime:
                    if (CompareValues(_player._stats.ReloadTime, value))
                    {
                        reloadTimer.Reset(value);
                    }
                    break;
                case StatType.FireRate:
                    if (CompareValues(_player._stats.FireRate, value))
                    {
                        fireRateTimer.Reset(value);
                    }
                    break;
            }
        }

        private bool CompareValues(float currentValue, float newValue)
        {
            return currentValue != newValue;
        }
    }
}
