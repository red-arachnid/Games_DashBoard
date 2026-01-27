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
        public int AudioReview { get; private init; }
        public int CreativityReview { get; private init; }
        public double FinalScore { get; private init; }

        public Game(Guid userId, int gameId, int gameplayReview, int storyReview, int visualReview, int audioReview, int creativityReview)
        {
            UserId = userId;
            IGDBGameId = gameId;
            GameplayReview = gameplayReview;
            StoryReview = storyReview;
            VisualReview = visualReview;
            AudioReview = audioReview;
            CreativityReview = creativityReview;
            FinalScore = (gameplayReview + storyReview + visualReview + audioReview + creativityReview) / 5;
        }
    }
}
