using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrawlingToys.UI
{
    public class SettingsController : MonoBehaviour
    {
        [Header("Toggles")]
        [SerializeField] private Toggle _fullScreenToggle;

        [Space]
        [Header("Dropdowns")]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private TMP_Dropdown _qualityDropdown;

        [Space]
        [Header("Sliders")]
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Slider _fpsSlider;

        private Resolution[] _resolutions;

        private void Start()
        {
            _masterVolumeSlider.onValueChanged.AddListener(MasterVolumeSliderOnValueChange_Handler);
            _musicVolumeSlider.onValueChanged.AddListener(MusicVolumeSliderOnValueChange_Handler);
            _sfxVolumeSlider.onValueChanged.AddListener(SfxVolumeSliderOnValueChange_Handler);
            _fpsSlider.onValueChanged.AddListener(SetTargetFPS);

            _resolutions = Screen.resolutions;

            _resolutionDropdown.ClearOptions();

            List<string> resolutionOptions = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                if ((float)_resolutions[i].width / 16.0f == (float)_resolutions[i].height / 9.0f)
                    resolutionOptions.Add(option);

                if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }

            _resolutionDropdown.AddOptions(resolutionOptions);

            LoadSettings();
        }

        private void OnDisable()
        {
            _masterVolumeSlider.onValueChanged.RemoveListener(MasterVolumeSliderOnValueChange_Handler);
            _musicVolumeSlider.onValueChanged.RemoveListener(MusicVolumeSliderOnValueChange_Handler);
            _sfxVolumeSlider.onValueChanged.RemoveListener(SfxVolumeSliderOnValueChange_Handler);
            _fpsSlider.onValueChanged.RemoveListener(SetTargetFPS);
        }

        // Volume controlado pelo Feel. Aqui � apenas para salvar com PlayerPrefs
        private void MasterVolumeSliderOnValueChange_Handler(float value)
        {
            PlayerPrefs.SetFloat("masterVolume", value);

            _masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        }

        private void MusicVolumeSliderOnValueChange_Handler(float value)
        {
            PlayerPrefs.SetFloat("musicVolume", value);

            _musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        private void SfxVolumeSliderOnValueChange_Handler(float value)
        {
            PlayerPrefs.SetFloat("sfxVolume", value);

            _sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }

        public void SetFullScreen(bool value)
        {
            Screen.fullScreen = value;

            if (value)
                PlayerPrefs.SetInt("fullScreen", 1);
            else
                PlayerPrefs.SetInt("fullScreen", 0);

            _fullScreenToggle.isOn = value;
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);

            _resolutionDropdown.value = PlayerPrefs.GetInt("resolutionIndex");
            _resolutionDropdown.RefreshShownValue();
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);

            PlayerPrefs.SetInt("qualityIndex", qualityIndex);

            _qualityDropdown.value = PlayerPrefs.GetInt("qualityIndex");
            _qualityDropdown.RefreshShownValue();
        }

        public void SetTargetFPS(float targetFPS)
        {
            _fpsSlider.minValue = 30;
            _fpsSlider.maxValue = 120;

            int int_targetFPS = Mathf.FloorToInt(targetFPS);

            Application.targetFrameRate = int_targetFPS;

            PlayerPrefs.SetInt("targetFPS", int_targetFPS);

            _fpsSlider.value = (float)PlayerPrefs.GetInt("targetFPS");
        }

        private void LoadSettings()
        {
            // Load Audio Settings
            if (PlayerPrefs.HasKey("masterVolume"))
                _masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
            else
                _masterVolumeSlider.value = _masterVolumeSlider.maxValue;

            if(PlayerPrefs.HasKey("musicVolume"))
                _musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
            else
                _musicVolumeSlider.value = _musicVolumeSlider.maxValue;

            if (PlayerPrefs.HasKey("sfxVolume"))
                _sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            else
                _sfxVolumeSlider.value = _sfxVolumeSlider.maxValue;

            // Load Screen Mode
            if (PlayerPrefs.HasKey("fullScreen"))
            {
                bool value = PlayerPrefs.GetInt("fullScreen") > 0 ? true : false;
                SetFullScreen(value);
            }
            else 
                SetFullScreen(true);

            // Load Resolution Settings
            if (PlayerPrefs.HasKey("resolutionIndex"))
                SetResolution(PlayerPrefs.GetInt("resolutionIndex"));
            else
                SetResolution(_resolutions.Length - 1);

            // Load Quality Settings
            if (PlayerPrefs.HasKey("qualityIndex"))
                SetQuality(PlayerPrefs.GetInt("qualityIndex"));
            else
                SetQuality(QualitySettings.count - 1);

            // Load FPS Settings
            if (PlayerPrefs.HasKey("targetFPS"))
                SetTargetFPS((float)PlayerPrefs.GetInt("targetFPS"));
            else
                SetTargetFPS(120f);
        }
    }
}
