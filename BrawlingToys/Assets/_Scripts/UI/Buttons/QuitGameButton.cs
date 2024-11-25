using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrawlingToys.UI
{
    [RequireComponent(typeof(Button))]
    public class QuitGameButton : MonoBehaviour
    {
        private Button _button; 

        private void Start()
        {
            _button = GetComponent<Button>(); 

            _button.onClick.AddListener(() =>
            {
                Application.Quit();
                Debug.Log("Game Quieted!");
            }); 
        }
    }
}
