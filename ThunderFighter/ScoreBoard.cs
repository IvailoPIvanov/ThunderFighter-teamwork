namespace ThunderFighter
{
    using ThunderFighter.Common.Enums;

    public class ScoreBoard
    {
        public ScoreBoard()
        {
        }

        public int Score { get; set; }

        public int HighestScore { get; set; }

        public int Lives { get; set; }

        public GameLevelType Level { get; set; }
    }
}