namespace BrawlingToys.Network
{
    [System.Serializable]
    public struct NetworkSerializedMatchInfo
    {
        public float BuffSpawnChance {  get; set; }
        public float DebuffSpawnChance { get; set; }
        public int PlayerLife { get; set; }
        public int RequiredPointsToWin { get; set; }

        public NetworkSerializedMatchInfo(float buffSpawnChance, float debuffSpawnChance, int playerLife, int requiredPointsToWin) 
        {
            BuffSpawnChance = buffSpawnChance;
            DebuffSpawnChance = debuffSpawnChance;
            PlayerLife = playerLife;
            RequiredPointsToWin = requiredPointsToWin;
        }
    }
}
