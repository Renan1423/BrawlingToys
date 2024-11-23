using TMPro;
using UnityEngine;

namespace BrawlingToys.UI
{
    public class UpdateSliderTextHelper : MonoBehaviour
    {
        [SerializeField] private bool canMultplyBy100 = true;

        [SerializeField] private TextMeshProUGUI _tmp;

        public void SliderOnValueChange_Handler(float value)
        {
            if(canMultplyBy100)
                _tmp.text = Mathf.FloorToInt(100 * value).ToString();
            else
                _tmp.text = Mathf.FloorToInt(value).ToString();
        }
    }
}
