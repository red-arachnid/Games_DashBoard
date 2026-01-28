namespace Games_DashBoard.Model
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public int IGDBGameId { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public int GameplayReview { get; set; }
        public int StoryReview { get; set; }
        public int VisualReview { get; set; }
        public int AudioReview { get; set; }
        public int CreativityReview { get; set; }
        public double FinalScore => (GameplayReview + StoryReview + VisualReview + AudioReview + CreativityReview) / 5;
    }
}
