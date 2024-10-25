using System;
using System.Collections.Generic;

namespace BrawlingToys.Actors
{
    public class StatsMediator
    {
        private readonly LinkedList<StatModifier> _modifiers = new();
        private List<ModifierScriptable> _appliedModifiers;

        public EventHandler<Query> Queries;

        public EventHandler<List<ModifierScriptable>> OnMediatorChange; 

        // Whenever we want to know about specific stats modifier we can perform a query.
        // Invoke all of the queries that we've stored inside of that queries variable.
        public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

        // Every time we add new modifier we are also going to register it with the queries
        public void AddModifier(ModifierScriptable modifierSo)
        {
            // Every time we perform a query that's going to be specific to a type of stat every single modifier is going to get its Handle()
            // called
            StatModifier statModifier = modifierSo.CreateModifier();

            _modifiers.AddLast(statModifier);
            Queries += statModifier.Handle;

            statModifier.OnDispose += _ =>
            {
                _modifiers.Remove(statModifier);
                _appliedModifiers.Remove(modifierSo);
                Queries -= statModifier.Handle;
            };

            if (_appliedModifiers == null)
                _appliedModifiers = new List<ModifierScriptable>();

            _appliedModifiers.Add(modifierSo);

            OnMediatorChange?.Invoke(this, _appliedModifiers); 
        }

        public void Update(float deltaTime)
        {
            // Chama o Update() de todos os modifiers com um deltaTime
            var node = _modifiers.First;
            while (node != null)
            {
                var modifier = node.Value;
                modifier.Update(deltaTime);
                node = node.Next;
            }

            // Remove todos os modifiers que ja tiverem acabado
            node = _modifiers.First;
            while (node != null)
            {
                var nextNode = node.Next;

                if (node.Value.MarkedForRemoval)
                {
                    node.Value.Dispose();
                }

                node = nextNode;
            }
        }

        public List<ModifierScriptable> GetAppliedModifiers() => _appliedModifiers;
    }
}
