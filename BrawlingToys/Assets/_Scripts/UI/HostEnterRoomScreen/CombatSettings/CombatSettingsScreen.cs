using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BrawlingToys.UI
{
    [System.Serializable]
    public struct CombatSettings
    {
        [field: SerializeField]
        public float BuffSpawnChance { get; private set; }
        [field: SerializeField]
        public float DebuffSpawnChance { get; private set; }
        [field: SerializeField]
        public int PlayerLife { get; private set; }
        [field: SerializeField]
        public int RequiredPointsToWin { get; private set; }

        public CombatSettings(float buffSpawnChance, float debuffSpawnChance, int playerLife, int requiredPointsToWin) 
        {
            BuffSpawnChance = buffSpawnChance;
            DebuffSpawnChance = debuffSpawnChance;
            PlayerLife = playerLife;
            RequiredPointsToWin = requiredPointsToWin;
        }
    }

    public class CombatSettingsScreen : BaseScreen
    {
        [Space(20)]

        [SerializeField]
        private CombatSettings initialCombatSettings;
        private CombatSettings currentCombatSettings;

        [SerializeField]
        private Slider buffDebuffChanceSlider;
        [SerializeField]
        private TextMeshProUGUI buffChanceText;
        [SerializeField]
        private TextMeshProUGUI debuffChanceText;
        [SerializeField]
        private Slider playersLifeSlider;
        [SerializeField]
        private TextMeshProUGUI playerLifeText;
        [SerializeField]
        private Slider requiredPointsToWinSlider;
        [SerializeField]
        private TextMeshProUGUI requiredPointsToWinText;

        protected override void Start()
        {
            base.Start();

            SetupDelegates();

            ResetSettings();
        }

        private void SetupDelegates() 
        {
            buffDebuffChanceSlider.onValueChanged.AddListener(delegate { OnSettingsValueChanged(buffDebuffChanceSlider.value, buffChanceText); });
            buffDebuffChanceSlider.onValueChanged.AddListener(delegate { OnSettingsValueChanged((100f - buffDebuffChanceSlider.value), debuffChanceText); });
            buffDebuffChanceSlider.onValueChanged.AddListener(delegate { buffChanceText.text += "%"; debuffChanceText.text += "%"; });

            playersLifeSlider.onValueChanged.AddListener(delegate { OnSettingsValueChanged(playersLifeSlider.value, playerLifeText); });

            requiredPointsToWinSlider.onValueChanged.AddListener(delegate { OnSettingsValueChanged(requiredPointsToWinSlider.value, requiredPointsToWinText); });
        }

        public void SaveSettings() 
        {
            float debuffChance = 100 - buffDebuffChanceSlider.value;
            float buffChance = 100 - debuffChance;

            CombatSettings combatSettings = new(buffChance, debuffChance, 
                (int) playersLifeSlider.value, 
                (int) requiredPointsToWinSlider.value);

            currentCombatSettings = combatSettings;
        }

        public void ResetSettings() 
        {
            buffDebuffChanceSlider.value = initialCombatSettings.BuffSpawnChance;
            playersLifeSlider.value = initialCombatSettings.PlayerLife;
            requiredPointsToWinSlider.value = initialCombatSettings.RequiredPointsToWin;

            currentCombatSettings = initialCombatSettings;
        }

        public CombatSettings GetCombatSettings() => currentCombatSettings;

        public void OnSettingsValueChanged(float sliderValue, TextMeshProUGUI outputText) 
        {
            outputText.text = sliderValue.ToString();
        }

        public void AddLifeValue(int value) 
        {
            playersLifeSlider.value += value;
        }

        public void AddRequiredPointsValue(int value) 
        {
            requiredPointsToWinSlider.value += value;
        }
    }
}
