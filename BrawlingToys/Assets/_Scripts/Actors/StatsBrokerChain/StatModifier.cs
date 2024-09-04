using BrawlingToys.Core;
using System;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public abstract class StatModifier : IDisposable
    {
        // The delegate operator creates an anonymous method that can be converted to a delegate type.
        // An anonymous method can be converted to types such as System.Action and System.Func<TResult> types used as arguments to many methods.
        public event Action<StatModifier> OnDispose = delegate { };

        public bool MarkedForRemoval { get; private set; }

        private readonly CountdownTimer _timer;

        // Passar valor 0 ou negativo de duração ocasionará a duração infinita desse modificador.
        protected StatModifier(float duration)
        {
            // Modificação permanente
            if (duration <= 0)
                return;

            _timer = new(duration);
            _timer.OnTimerStop += () => MarkedForRemoval = true;
            _timer.Start();
        }

        public void Update(float deltaTime) => _timer?.Tick(deltaTime);

        public abstract void Handle(object sender, Query query);

        public void Dispose()
        {
            // Cada StatModifier pode precisar de uma manipulação diferente quando os descartamos
            OnDispose?.Invoke(this);
        }
    }

    public class BasicStatModifier : StatModifier
    {
        private readonly StatType _type;
        private readonly Func<float, float> operation;

        public BasicStatModifier(StatType type, Func<float, float> operation, float duration) : base(duration)
        {
            _type = type;
            this.operation = operation;
        }

        public override void Handle(object sender, Query query)
        {
            if (query.StatType == _type)
            {
                query.Value = operation(query.Value);
                PlayerCooldownController.OnSomeCooldownChange?.Invoke(_type, query.Value);
            }
        }
    }

    public class BulletModifier : StatModifier
    {
        private readonly StatType _type;
        private readonly Func<GameObject, GameObject> operation;

        public BulletModifier(StatType type, Func<GameObject, GameObject> operation, float duration) : base(duration)
        {
            _type = type;
            this.operation = operation;
        }

        public override void Handle(object sender, Query query)
        {
            if (query.StatType == _type)
            {
                query.BulletEffect = operation(query.BulletEffect);
            }
        }
    }
}
