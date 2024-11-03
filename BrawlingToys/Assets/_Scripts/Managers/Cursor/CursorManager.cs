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
        [SerializeField]
        private Texture2D _gameplayCursor;

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

            GameManager.LocalInstance.OnGameStateChange.RemoveListener(OnGameStateChanged);
        }

        public void OnGameStateChanged(GameStateType newGameState) 
        {
            Texture2D cursorTex = (newGameState == GameStateType.Combat) ? _gameplayCursor : _uiCursor;

            Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.Auto);
        }
    }
}
