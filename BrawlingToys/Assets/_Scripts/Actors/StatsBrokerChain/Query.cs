using UnityEngine;

namespace BrawlingToys.Actors
{
    public class Query 
    {
        /// <summary>
        /// Query � uma solicita��o de informa��es a um banco de dados!
        /// O gerenciamento do banco de dados permite remover, acrescentar ou modificar os dados do seu projeto on-line. E para concretizar
        /// essas a��es, � necess�rio fazer uma Query. Mas para isso ocorrer, � importante que o banco de dados compreenda a solicita��o.
        /// Portanto, quando os comandos certos s�o executados, a pessoa desenvolvedora obt�m os resultados desejados
        /// de informa��es que j� est�o�armazenadas.
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
