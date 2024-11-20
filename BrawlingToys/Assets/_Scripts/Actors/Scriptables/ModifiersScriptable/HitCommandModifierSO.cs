using UnityEngine;

namespace BrawlingToys.Actors
{
    public enum HitCommandTypes {
        KillCommand,
        CollateralCommand
    }

    [CreateAssetMenu(fileName = "Hit Command Modifier", menuName = "Modifier/Hit Command Modifier")]
    public class HitCommandModifierSO : ModifierScriptable {

        public HitCommandTypes hitCommandTypes;

        private HitCommand SelectedHitType() {
            switch (hitCommandTypes) {
                case HitCommandTypes.KillCommand:
                    return new KillCommand();
                    
                case HitCommandTypes.CollateralCommand:
                    return new CollateralCommand();

                default:
                    Debug.LogError("Não era pra isso ter acontecido!");
                    return new KillCommand();
            }
        }

        public override StatModifier CreateModifier() {
            statModifier = new HitEffectModifier(StatType.HitCommand, value => SelectedHitType(), -1);

            return statModifier;
        }
    }
}
