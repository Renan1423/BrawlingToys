using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.UI
{
    public class Tutorial : BaseScreen
    {
        [Space(10)]

        private bool _isClosing;

        public void CloseTutorial() 
        {
            if (_isClosing)
                return;

            _isClosing = true;

            CloseScreen(0.25f);
        }
    }
}
