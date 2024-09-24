using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BrawlingToys.UI
{
    public class DrawerButton : MonoBehaviour
    {
        private float _unhoveredPosZ;
        [SerializeField]
        private float _hoveredPosZ;

        [Header("Main Menu reference")]
        [SerializeField]
        private MainMenu _mainMenu;

        private void Start()
        {
            _unhoveredPosZ = transform.position.z;
        }

        public void OnHoverDrawer(bool result) 
        {
            if (result && !_mainMenu.gameObject.activeInHierarchy)
                return;

            float targetPosX = (result) ? _hoveredPosZ : _unhoveredPosZ;
            transform.DOMoveZ(targetPosX, 0.5f);
        }
    }
}
