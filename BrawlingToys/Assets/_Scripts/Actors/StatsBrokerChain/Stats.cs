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
        Bullet
    }

    public class Stats
    {
        private readonly StatsMediator _mediator;
        private readonly BaseStats _baseStats;

        public StatsMediator Mediator => _mediator;

        public float MoveSpeed
        {
            get
            {
                // return value with modifiers applied
                var query = new Query(StatType.MoveSpeed, _baseStats.moveSpeed);
                _mediator.PerformQuery(this, query);
                return query.Value;
            }
        }

        public float MeleeCooldown
        {
            get
            {
                // return value with modifiers applied
                var query = new Query(StatType.MeleeCooldown, _baseStats.meleeCooldown);
                _mediator.PerformQuery(this, query);
                return query.Value;
            }
        }
        public float DashCooldown
        {
            get
            {
                // return value with modifiers applied
                var query = new Query(StatType.DashCooldown, _baseStats.dashCooldown);
                _mediator.PerformQuery(this, query);
                return query.Value;
            }
        }

        public int DashAmount
        {
            get
            {
                // return value with modifiers applied
                var query = new Query(StatType.DashAmount, _baseStats.dashAmount);
                _mediator.PerformQuery(this, query);
                return (int)query.Value;
            }
        }

        public float ReloadTime
        {
            get
            {
                // return value with modifiers applied
                var query = new Query(StatType.ReloadTime, _baseStats.reloadTime);
                _mediator.PerformQuery(this, query);
                return query.Value;
            }
        }

        public float FireRate
        {
            get
            {
                // return value with modifiers applied
                var query = new Query(StatType.FireRate, _baseStats.fireRate);
                _mediator.PerformQuery(this, query);
                return query.Value;
            }
        }

        public int BulletAmount
        {
            get
            {
                // return value with modifiers applied
                var query = new Query(StatType.FireRate, _baseStats.bulletAmount);
                _mediator.PerformQuery(this, query);
                return (int)query.Value;
            }
        }

        public GameObject CurrentBullet
        {
            get
            {
                // return value with modifiers applied
                var query = new Query(StatType.Bullet, 0, _baseStats.defaultBullet);
                _mediator.PerformQuery(this, query);
                return query.BulletEffect;
            }
        }

        public Stats(StatsMediator mediator, BaseStats baseStats)
        {
            _mediator = mediator;
            _baseStats = baseStats;
        }
    }
}
