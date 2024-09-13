namespace BrawlingToys.Core
{
    public class PlayerRoundInfo
    {
        public int KillsAmount { get; set; }
        public bool IsSurvivor { get; set; }

        public PlayerRoundInfo(int killsAmount, bool isSurvivor)
        {
            KillsAmount = killsAmount;
            IsSurvivor = isSurvivor;
        }
    }
}