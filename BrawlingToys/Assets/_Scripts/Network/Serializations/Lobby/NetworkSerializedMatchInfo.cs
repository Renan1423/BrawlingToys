namespace BrawlingToys.Network
{
    [System.Serializable]
    public struct NetworkSerializedMatchInfo
    {
        public float BuffSpawnChance;
        public float DebuffSpawnChance;
        public int PlayerLife;
        public int RequiredPointsToWin;

        public NetworkSerializedMatchInfo(float buffSpawnChance, float debuffSpawnChance, int playerLife, int requiredPointsToWin) 
        {
            BuffSpawnChance = buffSpawnChance;
            DebuffSpawnChance = debuffSpawnChance;
            PlayerLife = playerLife;
            RequiredPointsToWin = requiredPointsToWin;
        }
    }
}
