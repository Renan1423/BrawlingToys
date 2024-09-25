using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BrawlingToys.Actors;
using BrawlingToys.UI;
using BrawlingToys.Managers;
using Unity.Netcode;
using System.Linq;

public class EffectsSelectionScreen : BaseScreen
{
    [SerializeField]
    private GameObject _playerInfoPrefab;
    [SerializeField]
    private Transform _playerInfoHorizontalLayout;
    [SerializeField]
    //Variable created for prototype reasons only!
    private AssetReference _playerCharacterAssetRef;

    private ModifierScriptable _drawnEffect;
    [SerializeField]
    private List<Player> _players;
    //Variable created for prototype reasons only!
    private Stats _playerStats;

    protected override void OnScreenEnable()
    {
        GetPlayersReferenceServerRpc(); 
        GetPlayersInformation();
    }

    public void GetPlayersInformation()
    {
        //We must gather the player informations using the multiplayer features. This is only for prototype purpose
        for (int i = 0; i < _players.Count; i++)
        {
            string playerName = "Player " + i;
            SpawnPlayerInfo(_drawnEffect, _players[i].Stats, playerName, _playerCharacterAssetRef, new GameObject[0]);
        }
    }

    public void SpawnPlayerInfo(ModifierScriptable effectToApply, Stats playerStats, string playerName, AssetReference characterAsset, GameObject[] effectsGo) 
    {
        GameObject playerInfoGo = Instantiate(_playerInfoPrefab, _playerInfoHorizontalLayout);
        PlayerInfoPanel playerInfo = playerInfoGo.GetComponent<PlayerInfoPanel>();

        playerInfo.FillInfoPanel(effectToApply, playerStats, playerName, characterAsset, effectsGo);
        playerInfo.GetPlayerInfoClickEvent().AddListener(OnTargetSelected);
    }

    public void SetDrawnEffect(ModifierScriptable drawnEffect) => _drawnEffect = drawnEffect;

    public void OnTargetSelected(PlayerInfoPanel playerInfoPanel) 
    {
        Debug.Log("Target selected");
        ScreenManager.instance.ToggleScreenByTag(ScreenName, false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void GetPlayersReferenceServerRpc()
    {
        var playersIds = MatchManager.LocalInstance.MatchPlayers
            .Select(p => p.PlayerId)
            .ToArray(); 

        GetPlayersReferenceClientRpc(playersIds); 
    }

    [ClientRpc]
    private void GetPlayersReferenceClientRpc(ulong[] playersIds)
    {
        foreach (var id in playersIds)
        {
            if (NetworkManager.Singleton.ConnectedClients.ContainsKey(id))
            {
                var playerNetworkObject = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
                var playerReference = playerNetworkObject.GetComponent<Player>(); 
                
                _players.Add(playerReference);
            }
        }
    }
}
