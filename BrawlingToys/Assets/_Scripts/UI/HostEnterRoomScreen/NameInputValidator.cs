using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace BrawlingToys.UI
{
    public class NameInputValidator : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _nameInputField;
        [SerializeField]
        private Animator _errorAnim;
        [SerializeField]
        private Animator _inputFieldAnim;

        public string InputFieldText => _nameInputField.text;

        public void ValidateName() 
        {
            _nameInputField.text = new string(_nameInputField.text.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                    .ToArray());
        }

        public bool CheckNameValidation() 
        {
            ValidateName();

            bool validationResult = _nameInputField.text != "";

            if (!validationResult) 
            {
                _errorAnim.SetTrigger("Show");
                _inputFieldAnim.SetTrigger("Error");
            }

            return validationResult;
        }
    }
}
