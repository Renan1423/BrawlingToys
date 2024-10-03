using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;

namespace BrawlingToys.UI
{
    public class DrawerButton : MonoBehaviour
    {
        private float _unhoveredPosZ;
        [SerializeField]
        private float _hoveredPosZ;
        [Header("Feedbacks")]
        [SerializeField]
        private MMF_Player _hoverFeedback;
        [SerializeField]
        private MMF_Player _outlineFeedback;
        private bool _outlineOn;

        [Header("Main Menu reference")]
        [SerializeField]
        private MainMenu _mainMenu;

        private void Start()
        {
            _unhoveredPosZ = transform.position.z;
        }

        public void OnHoverDrawer(bool result) 
        {
            if (!_mainMenu.gameObject.activeInHierarchy)
                return;

            _hoverFeedback.PlayFeedbacks();
            ToggleOutline(result);

            float targetPosX = (result) ? _hoveredPosZ : _unhoveredPosZ;
            transform.DOMoveZ(targetPosX, 0.5f);
        }

        private void ToggleOutline(bool result) 
        {
            if ((result && !_outlineOn) || 
                (!result && _outlineOn)) 
            {
                _outlineFeedback.PlayFeedbacks();
                _outlineOn = !_outlineOn;
            }
        }
    }
}
