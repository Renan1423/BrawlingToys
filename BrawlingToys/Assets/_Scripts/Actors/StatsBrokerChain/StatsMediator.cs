using System;
using System.Collections.Generic;

namespace BrawlingToys.Actors
{
    public class StatsMediator
    {
        private readonly LinkedList<StatModifier> modifiers = new();

        public EventHandler<Query> Queries;

        // Whenever we want to know about specific stats modifier we can perform a query.
        // Invoke all of the queries that we've stored inside of that queries variable.
        public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

        // Every time we add new modifier we are also going to register it with the queries
        public void AddModifier(StatModifier modifier)
        {
            // Every time we perform a query that's going to be specific to a type of stat every single modifier is going to get its Handle()
            // called
            modifiers.AddLast(modifier);
            Queries += modifier.Handle;

            modifier.OnDispose += _ =>
            {
                modifiers.Remove(modifier);
                Queries -= modifier.Handle;
            };
        }

        public void Update(float deltaTime)
        {
            // Chama o Update() de todos os modifiers com um deltaTime
            var node = modifiers.First;
            while (node != null)
            {
                var modifier = node.Value;
                modifier.Update(deltaTime);
                node = node.Next;
            }

            // Remove todos os modifiers que ja tiverem acabado
            node = modifiers.First;
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
    }
}
