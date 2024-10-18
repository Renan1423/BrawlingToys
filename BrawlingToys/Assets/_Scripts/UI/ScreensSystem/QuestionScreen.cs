using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BrawlingToys.UI
{
    public class QuestionScreen : BaseScreen
    {
        [Space(10)]

        [SerializeField]
        private TextMeshProUGUI _questionText;
        [SerializeField]
        private TextMeshProUGUI _questionTextShadow;
        [SerializeField]
        private Button _yesButton;
        [SerializeField]
        private Button _noButton;

        public void InitQuestion(string Question, UnityEngine.Events.UnityAction Yes_Function, UnityEngine.Events.UnityAction No_Function)
        {
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();

            _questionText.text = Question;
            if (_questionTextShadow != null)
                _questionTextShadow.text = _questionText.text;

            _yesButton.onClick.AddListener(Yes_Function);
            _yesButton.onClick.AddListener(AnswerSelected);
            _noButton.onClick.AddListener(No_Function);
            _noButton.onClick.AddListener(AnswerSelected);
        }

        private void AnswerSelected()
        {
            CloseScreen(0.15f);
        }
    }
}
