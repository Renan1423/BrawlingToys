using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public enum EffectType
    {
        Buff,
        Debuff
    }

    public abstract class ModifierScriptable : ScriptableObject
    {
        [field:SerializeField] public string Tag { get; private set; }
        [field:SerializeField] public StatType ModifierType { get; private set; }

        [field: SerializeField] public string EffectName { get; private set; }

        [field: SerializeField] public string EffectDescription { get; private set; }

        [field: SerializeField] public Sprite EffectIcon { get; private set; }

        [field: SerializeField] public EffectType EffectType { get; private set; }

        protected StatModifier statModifier;

        public abstract StatModifier CreateModifier();
    }
}
