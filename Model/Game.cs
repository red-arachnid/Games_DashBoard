namespace Games_DashBoard.Model
{
    public class Game
    {
        public Guid Id { get; private init; } = Guid.NewGuid();
        public Guid UserId { get; private init; }
        public int IGDBGameId { get; private init; } //Check the id with IGDBGameData for game Info
        public int GameplayReview { get; private init; }
        public int StoryReview { get; private init; }
        public int VisualReview { get; private init; }
        public int PerformanceReview { get; private init; }
        public int AudioReview { get; private init; }
        public int ExperienceReview { get; private init; }
        public int AtmosphereReview { get; private init; }
        public int CreativityReview { get; private init; }
        public int ReplayabilityReview { get; private init; }
        public double FinalScore { get; private init; }

        public Game(Guid userId, int gameId, int gameplayReview, int visualReview, int performanceReview, int audioReview,
            int experienceReview, int atmosphereReview, int creativityReview, int replayabilityReview)
        {
            UserId = userId;
            IGDBGameId = gameId;
            GameplayReview = gameplayReview;
            VisualReview = visualReview;
            PerformanceReview = performanceReview;
            AudioReview = audioReview;
            ExperienceReview = experienceReview;
            AtmosphereReview = atmosphereReview;
            CreativityReview = creativityReview;
            ReplayabilityReview = replayabilityReview;
            FinalScore = (gameplayReview + visualReview + performanceReview + audioReview + experienceReview + atmosphereReview + creativityReview + replayabilityReview) / 9;
        }
    }
}
