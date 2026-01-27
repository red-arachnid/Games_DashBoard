namespace Games_DashBoard.Model
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public int IGDBGameId { get; set; } //Check the id with IGDBGameData for game Info
        public int GameplayReview { get; set; }
        public int StoryReview { get; set; }
        public int VisualReview { get; set; }
        public int AudioReview { get; set; }
        public int CreativityReview { get; set; }
        public double FinalScore => (GameplayReview + StoryReview + VisualReview + AudioReview + CreativityReview) / 5;

        /*
        public Game(Guid userId, int igdbGameId, int gameplayReview, int storyReview, int visualReview, int audioReview, int creativityReview)
        {
            UserId = userId;
            IGDBGameId = igdbGameId;
            GameplayReview = gameplayReview;
            StoryReview = storyReview;
            VisualReview = visualReview;
            AudioReview = audioReview;
            CreativityReview = creativityReview;
            FinalScore = 
        }
        */

    }
}
