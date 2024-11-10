using UnityEngine;

namespace BrawlingToys.Actors
{
    [CreateAssetMenu(fileName = "New Basic Sum Modifier", menuName = "Modifier/Basic Sum Modifier")]
    public class BasicSumModifierSO : ModifierScriptable
    {
        public int modificationAddtion;

        public override StatModifier CreateModifier()
        {
            statModifier = new BasicStatModifier(ModifierType, value => value + modificationAddtion, -1);
            return statModifier;
        }
    }
}
