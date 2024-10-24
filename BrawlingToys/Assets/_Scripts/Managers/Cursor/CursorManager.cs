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

        private void Start()
        {
            GameManager.LocalInstance.OnGameStateChange.AddListener(OnGameStateChanged);
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
