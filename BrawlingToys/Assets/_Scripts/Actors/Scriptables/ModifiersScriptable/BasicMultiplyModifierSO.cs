using UnityEngine;

namespace BrawlingToys.Actors
{
    [CreateAssetMenu(fileName = "New Basic Multiply Modifier", menuName = "Modifier/Basic Multply Modifier")]
    public class BasicMultiplyModifierSO : ModifierScriptable
    {
        public float modificationMultiplier;

        public override StatModifier CreateModifier()
        {
            statModifier = new BasicStatModifier(ModifierType, value => value * modificationMultiplier, -1);
            return statModifier;
        }
    }
}
