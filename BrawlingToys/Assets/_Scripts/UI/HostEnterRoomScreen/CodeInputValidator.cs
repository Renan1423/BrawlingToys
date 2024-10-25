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
        [SerializeField]
        private Animator _inputFieldAnim;

        public string InputFieldText => _codeText.text;

        public void ValidateCode() 
        {
            _codeText.text = _codeText.text.ToUpper();

            _codeText.text = new string(_codeText.text.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                    .ToArray());
        }

        public void ShowCodeInputFieldError() 
        {
            _errorAnim.SetTrigger("Show");
            _inputFieldAnim.SetTrigger("Error");
        }
    }
}
