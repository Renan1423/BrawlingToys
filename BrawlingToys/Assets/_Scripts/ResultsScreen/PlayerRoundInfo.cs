using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public struct PlayerRoundInfo
{
    public int PlayerNumber { get; private set; }
    public AssetReference PlayerCharacter { get; private set; }
    public int KillsAmount { get; private set; }
    public bool IsSurvivor { get; private set; }

    public PlayerRoundInfo(int playerNumber, AssetReference playerCharacter, int killsAmount, bool isSurvivor) 
    {
        PlayerNumber = playerNumber;
        PlayerCharacter = playerCharacter;
        KillsAmount = killsAmount;
        IsSurvivor = isSurvivor;
    }
}
