using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Managers;
using BrawlingToys.Core;

namespace BrawlingToys.UI
{
    public class HostEnterRoomSelectionScreen : BaseScreen
    {
        public void OpenCreateRoomScreen() 
        {
            ScreenManager.instance.ToggleScreenByTag(TagManager.CreateRoomMenu.CREATE_ROOM, true);
            CloseScreen(0f);
        }

        public void OpenJoinRoomScreen() 
        {
            ScreenManager.instance.ToggleScreenByTag(TagManager.CreateRoomMenu.JOIN_ROOM, true);
            CloseScreen(0f);
        }
    }
}
