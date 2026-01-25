namespace Games_DashBoard.Model
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public IGDBGameData? GameData { get; set; }
        public int GameplayReview { get; set; }
        public int StoryReview { get; set; }
        public int VisualReview { get; set; }
        public int PerformanceReview { get; set; }
        public int AudioReview { get; set; }
        public int ExperienceReview { get; set; }
        public int AtmosphereReview { get; set; }
        public int CreativityReview { get; set; }
        public int ReplayabilityReview { get; set; }
        public double FinalScore => (GameplayReview + StoryReview + VisualReview + PerformanceReview + AudioReview + ExperienceReview + AtmosphereReview + CreativityReview + ReplayabilityReview) / 9;
    }
}
