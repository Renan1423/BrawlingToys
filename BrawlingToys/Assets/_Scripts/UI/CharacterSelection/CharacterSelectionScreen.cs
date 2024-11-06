using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BrawlingToys.Managers;
using BrawlingToys.Core;

namespace BrawlingToys.UI
{
    [System.Serializable]
    public struct CharacterSelectionData
    {
        public string CharacterName;
        public AssetReference CharacterModel;
        public Sprite CharacterIcon;
        public Color CharacterColor;
    }

    [System.Serializable]
    public struct ChosenCharacterData
    {
        public string CharacterName { get; private set; }
        public AssetReference ChosenCharacterPrefab { get; private set; }
        public Sprite CharacterIcon { get; private set; }

        public ChosenCharacterData(string characterName, AssetReference chosenCharacterPrefab, Sprite characterIcon)
        {
            CharacterName = characterName;
            ChosenCharacterPrefab = chosenCharacterPrefab;
            CharacterIcon = characterIcon;
        }
    }

    public class CharacterSelectionScreen : BaseScreen
    {
        [SerializeField]
        private List<CharacterSelectionData> _playableCharacters;
        private List<CharacterButton> _characterButtons;
        private ChosenCharacterData _chosenCharacter;
        private int _selectedCharacterIndex = -1;

        [Space(20)]

        [Header("UI")]
        [SerializeField]
        private RawImage _characterDisplayRawImage;
        [SerializeField]
        private Animator _characterDisplayAnim;
        [SerializeField]
        private TextMeshProUGUI _characterNameText;
        [SerializeField]
        private BackgroundColorChanger _backgroundColorChanger;

        [Space(20)]

        [Header("Buttons")]
        [SerializeField]
        private GameObject _characterButtonPrefab;
        [SerializeField]
        private Transform _buttonsHorizontalLayout;

        protected override void Start()
        {
            base.Start();

            _characterButtons = new List<CharacterButton>();

            SetupCharacterButtons();
            SetupButtonsNavigation();
        }

        private void SetupCharacterButtons() 
        {
            foreach (CharacterSelectionData character in _playableCharacters)
            {
                GameObject characterButtonGO = Instantiate(_characterButtonPrefab, _buttonsHorizontalLayout);
                int characterIndex = _playableCharacters.IndexOf(character);

                //Making the setup of the character informations on the button
                CharacterButton characterButton = characterButtonGO.GetComponent<CharacterButton>();
                characterButton.SetupCharacterButton(characterIndex,
                    character.CharacterIcon, this);

                _characterButtons.Add(characterButton);
            }
        }

        private void SetupButtonsNavigation() 
        {
            for (int i = 0; i < _characterButtons.Count; i++)
            {
                //Setting the navigation of the buttons
                Button btn = _characterButtons[i].GetComponent<Button>();

                Navigation nav = new Navigation();
                nav.mode = Navigation.Mode.Explicit;
                nav.selectOnRight = (i < _characterButtons.Count - 1) ? _characterButtons[i + 1].GetComponent<Button>() : _characterButtons[0].GetComponent<Button>();
                nav.selectOnLeft = (i > 0) ? _characterButtons[i - 1].GetComponent<Button>() : _characterButtons[^1].GetComponent<Button>();

                btn.navigation = nav;
            }
        }

        public void ShowCharacter(int characterIndex) 
        {
            if (_selectedCharacterIndex == characterIndex)
                return;

            if (!ModelSpawner.Instance.HasActiveRenderTextureCamera())
                ModelSpawner.Instance.SpawnRenderTextureModelWithNewCamera(_playableCharacters[characterIndex].CharacterModel, 
                    _characterDisplayRawImage);
            else
                ModelSpawner.Instance.ReplaceExistingRenderTextureCamera(0, 
                    _playableCharacters[characterIndex].CharacterModel,
                    _characterDisplayRawImage);

            _characterNameText.text = _playableCharacters[characterIndex].CharacterName;
            _selectedCharacterIndex = characterIndex;

            _backgroundColorChanger.SetBackgroundColor(_playableCharacters[characterIndex].CharacterColor, 0.25f);
            _characterDisplayAnim.SetTrigger("Show");
        }

        public void SelectCharacter() 
        {
            QuestionScreen questionScreen = GameplayUiContainer.instance.QuestionScreen;
            ScreenManager.instance.ToggleScreenByTag(TagManager.MainMenu.QUESTION_SCREEN, true);
            questionScreen.InitQuestion("Escolher esse personagem?", ConfirmCharacterSelection, CancelCharacterSelection);
        }

        private void ConfirmCharacterSelection() 
        {
            CharacterSelectionData character = _playableCharacters[_selectedCharacterIndex];
            _chosenCharacter = new ChosenCharacterData(character.CharacterName, character.CharacterModel, character.CharacterIcon);

            _backgroundColorChanger.ResetBackgroundColor(0.25f);

            OpenNextScreen();
        }

        public ChosenCharacterData GetChosenCharacterData() 
        {
            return _chosenCharacter;
        }

        private void OpenNextScreen() 
        {
            ScreenManager.instance.ToggleScreenByTag(TagManager.CreateRoomMenu.HOST_ENTER_SELECTION, true);
            CloseScreen(0f);
        }

        private void CancelCharacterSelection() { }


        //Triggers the position of the RawImage that shows the character's entire body
        public void ToggleSkinView() 
        { 
            
        }
    }
}
