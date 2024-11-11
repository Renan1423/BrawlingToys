using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Network;

namespace BrawlingToys.Managers
{
    public class CursorManager : NetworkSingleton<CursorManager>
    {
        [SerializeField]
        private Texture2D _uiCursor;

        protected override void Awake()
        {
            base.Awake();

            GameManager.LocalInstance.OnGameStateChange.AddListener(OnGameStateChanged);

            Texture2D cursorTex = _uiCursor;
            Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.Auto);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public void OnGameStateChanged(GameStateType newGameState) 
        {
            if (newGameState == GameStateType.Combat) 
            {
                Cursor.visible = false;
                return;
            }
                
            Texture2D cursorTex = _uiCursor;

            Cursor.visible = true;
            Cursor.SetCursor(cursorTex, new Vector2(32, 32), CursorMode.Auto);
        }
    }
}
