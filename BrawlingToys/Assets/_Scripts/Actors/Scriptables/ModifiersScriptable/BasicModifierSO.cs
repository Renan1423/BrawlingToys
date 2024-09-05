using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.Actors
{
    [CreateAssetMenu(fileName = "New Basic Modifier", menuName = "Modifier/Basic Modifier")]
    public class BasicModifierSO : ModifierScriptable
    {
        public float modificationPercentage;

        public override StatModifier CreateModifier()
        {
            statModifier = new BasicStatModifier(ModifierType, value => value * modificationPercentage, -1);
            return statModifier;
        }
    }
}
