namespace BrawlingToys.Network
{
    [System.Serializable]
    public struct NetworkSerializedPlayerInfo 
    {
        public string PlayerName {  get; set; }
        public ulong PlayerId { get; set; }
        public string CharacterAssetGUID { get; set; }

        public NetworkSerializedPlayerInfo(string playerName, ulong playerId, string characterAssetGUID)
        {
            PlayerName = playerName;
            PlayerId = playerId;
            CharacterAssetGUID = characterAssetGUID;
        }
    }
}
