using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Actors;
using BrawlingToys.Network;

namespace BrawlingToys.Managers
{
    public class PlayerClientDatasManager : NetworkSingleton<PlayerClientDatasManager>
    {
        public List<PlayerClientData> PlayerClientDatas { get; private set; }

        public void AddPlayerClientData(PlayerClientData playerClientData) 
        {
            if (PlayerClientDatas == null)
                PlayerClientDatas = new List<PlayerClientData>();

            PlayerClientDatas.Add(playerClientData);
        }

        public void RemovePlayerClientData(PlayerClientData playerClientData) 
        {
            if (PlayerClientDatas == null)
                return;

            PlayerClientDatas.Remove(playerClientData);
        }
    }
}
