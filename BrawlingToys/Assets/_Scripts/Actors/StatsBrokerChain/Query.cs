using UnityEngine;

namespace BrawlingToys.Actors
{
    public class Query 
    {
        /// <summary>
        /// Query é uma solicitação de informações a um banco de dados!
        /// O gerenciamento do banco de dados permite remover, acrescentar ou modificar os dados do seu projeto on-line. E para concretizar
        /// essas ações, é necessário fazer uma Query. Mas para isso ocorrer, é importante que o banco de dados compreenda a solicitação.
        /// Portanto, quando os comandos certos são executados, a pessoa desenvolvedora obtém os resultados desejados
        /// de informações que já estão armazenadas.
        /// </summary>

        public readonly StatType StatType; // Type of stat that gets mutated by everything in the chain
        public float Value; // Value that gets mutated by everything in the chain
        public IHitCommand HitEffect;

        public Query(StatType statType, float value, IHitCommand hitEffect = null)
        {
            StatType = statType;
            Value = value;
            HitEffect = hitEffect;
        }
    }
}
