using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.Core
{
    public static class TagManager
    {
        public static class PlayerUiController 
        {
            public const string CLICK = "OnClick";
        }

        public static class MainMenu 
        {
            public const string MAIN_MENU = "MainMenu";
            public const string CREDITS = "Credits";
            public const string QUESTION_SCREEN = "QuestionScreen";
        }

        public static class CreateRoomMenu 
        { 
            public const string HOST_ENTER_SELECTION = "HostEnterRoomSelection";
            public const string CREATE_ROOM = "CreateRoom";
            public const string JOIN_ROOM = "JoinRoom";
            public const string WAITING_FOR_PLAYERS = "WaitingForPlayers";
            public const string ROOM_SETTINGS = "RoomSettings";
            public const string CLIENT_WAITING_ROOM = "ClientWaiting";
        }
    }
}
