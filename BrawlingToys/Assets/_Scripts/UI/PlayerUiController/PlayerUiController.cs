using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using BrawlingToys.DesignPatterns;
using BrawlingToys.Core;

namespace BrawlingToys.UI
{
    public class PlayerUiController : Subject
    {
        public void PlayerClick(InputAction.CallbackContext value) 
        {
            if (value.performed || value.canceled)
            {
                Debug.Log("PlayerUiController: Clicked!");
                Notify(TagManager.PlayerUiController.CLICK);
            }

        }
    }
}
