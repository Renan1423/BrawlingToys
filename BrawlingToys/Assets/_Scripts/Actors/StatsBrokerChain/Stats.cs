using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public enum StatType
    {
        MoveSpeed,
        MeleeCooldown,
        DashCooldown,
        DashAmount,
        ReloadTime,
        FireRate,
        BulletAmount,
        HitCommand
    }

    public class Stats
    {
        private readonly StatsMediator _mediator;
        private readonly BaseStats _baseStats;

        public event Action<StatType> OnStatsChanged;

        public StatsMediator Mediator => _mediator;

        public float MoveSpeed { get; private set; }
        //{
        //    get
        //    {
        //        // return value with modifiers applied
        //        var query = new Query(StatType.MoveSpeed, _baseStats.moveSpeed);
        //        _mediator.PerformQuery(this, query);
        //        return query.Value;
        //    }
        //}

        public float MeleeCooldown { get; private set; }
        //{
        //    get
        //    {
        //        // return value with modifiers applied
        //        var query = new Query(StatType.MeleeCooldown, _baseStats.meleeCooldown);
        //        _mediator.PerformQuery(this, query);
        //        return query.Value;
        //    }
        //}

        public float DashCooldown {  get; private set; }
        //{
        //    get
        //    {
        //        // return value with modifiers applied
        //        var query = new Query(StatType.DashCooldown, _baseStats.dashCooldown);
        //        _mediator.PerformQuery(this, query);
        //        return query.Value;
        //    }
        //}

        public int DashAmount { get; private set; }
        //{
        //    get
        //    {
        //        // return value with modifiers applied
        //        var query = new Query(StatType.DashAmount, _baseStats.dashAmount);
        //        _mediator.PerformQuery(this, query);
        //        return Mathf.FloorToInt(query.Value);
        //    }
        //}

        public float ReloadTime { get; private set; }
        //{
        //    get
        //    {
        //        // return value with modifiers applied
        //        var query = new Query(StatType.ReloadTime, _baseStats.reloadTime);
        //        _mediator.PerformQuery(this, query);
        //        return query.Value;
        //    }
        //}

        public float FireRate { get; private set; }
        //{
        //    get
        //    {
        //        // return value with modifiers applied
        //        var query = new Query(StatType.FireRate, _baseStats.fireRate);
        //        _mediator.PerformQuery(this, query);
        //        return query.Value;
        //    }
        //}

        public int BulletAmount { get; private set; }
        //{
        //    get
        //    {
        //        // return value with modifiers applied
        //        var query = new Query(StatType.FireRate, _baseStats.bulletAmount);
        //        _mediator.PerformQuery(this, query);
        //        return Mathf.FloorToInt(query.Value);
        //    }
        //}

        public HitCommand CurrentHitEffect {  get; private set; }
        //{
        //    get
        //    {
        //        // return value with modifiers applied
        //        var query = new Query(StatType.HitCommand, 0, _baseStats.defaultHitEffect);
        //        _mediator.PerformQuery(this, query);
        //        return query.HitEffect;
        //    }
        //}

        public Stats(StatsMediator mediator, BaseStats baseStats)
        {
            _mediator = mediator;
            _baseStats = baseStats;

            MoveSpeed = _baseStats.moveSpeed;
            MeleeCooldown = _baseStats.meleeCooldown;
            DashCooldown = _baseStats.dashCooldown;
            DashAmount = Mathf.FloorToInt(_baseStats.dashAmount);
            ReloadTime = _baseStats.reloadTime;
            FireRate = _baseStats.fireRate;
            CurrentHitEffect = _baseStats.defaultHitEffect;
        }

        public void ModifyStat(ModifierScriptable modifier, float valueToChange)
        {
            _mediator.AddModifier(modifier);

            var query = new Query(modifier.ModifierType, valueToChange);
            _mediator.PerformQuery(this, query);
            valueToChange = query.Value;
            SetNewStat(modifier.ModifierType, valueToChange, null);

            OnStatsChanged?.Invoke(modifier.ModifierType);
        }

        public void ModifyStat(ModifierScriptable modifier, HitCommand hitCommand)
        {
            _mediator.AddModifier(modifier);

            var query = new Query(modifier.ModifierType, 0, hitCommand);
            _mediator.PerformQuery(this, query);
            hitCommand = query.HitEffect;
            SetNewStat(modifier.ModifierType, 0, hitCommand);

            OnStatsChanged?.Invoke(modifier.ModifierType);
        }

        private void SetNewStat(StatType statType, float value, HitCommand hitCommand)
        {
            switch (statType)
            {
                case StatType.MoveSpeed: 
                    MoveSpeed = value;
                    break;
                case StatType.MeleeCooldown:
                    MeleeCooldown = value;
                    break;
                case StatType.DashCooldown:
                    DashCooldown = value;
                    break;
                case StatType.DashAmount:
                    DashAmount = Mathf.FloorToInt(value);
                    break;
                case StatType.ReloadTime:
                    ReloadTime = value;
                    break;
                case StatType.FireRate:
                    FireRate = value;
                    break;
                case StatType.HitCommand:
                    CurrentHitEffect = hitCommand;
                    break;
            }
        }
    }
}
