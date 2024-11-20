using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BrawlingToys.UI
{
    public class UpdateSliderTextHelper : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmp;

        public void SliderOnValueChange_Handler(float value)
        {
            _tmp.text = Mathf.FloorToInt(100 * value).ToString();
        }
    }
}
