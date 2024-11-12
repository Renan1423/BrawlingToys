namespace BrawlingToys.Network
{
    [System.Serializable]
    public struct NetworkSerializedPlayerInfo 
    {
        public string PlayerName;
        public ulong PlayerId;
        public string CharacterAssetGUID;

        public NetworkSerializedPlayerInfo(string playerName, ulong playerId, string characterAssetGUID)
        {
            PlayerName = playerName;
            PlayerId = playerId;
            CharacterAssetGUID = characterAssetGUID;
        }
    }
}
