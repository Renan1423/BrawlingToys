using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace BrawlingToys.UI
{
    public class CodeInputValidator : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _codeText;
        [SerializeField]
        private Animator _errorAnim;

        public void ValidateCode() 
        {
            _codeText.text = _codeText.text.ToUpper();

            _codeText.text = new string(_codeText.text.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                    .ToArray());
        }

        public bool CheckCodeValidation(string roomCode) 
        {
            ValidateCode();

            bool validationResult = _codeText.text.ToUpper() == roomCode.ToUpper();

            if (!validationResult)
                _errorAnim.SetTrigger("Show");

            return validationResult;
        }
    }
}
