using Games_DashBoard.Data;
using Games_DashBoard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games_DashBoard.Services
{
    public class GameService
    {
        private Repository _repository;
        private StoredData _data;

        public GameService()
        {
            _repository = new Repository();
            _data = _repository.LoadData();
        }

        public List<Game> GetLibraryOfUser(Guid userId)
            => _data.Games.Where(game => game.UserId == userId).ToList();

        public bool AddNewGame(Guid userId, int gameId, int gameplayReview, int visualReview, int performanceReview, int audioReview,
            int experienceReview, int atmosphereReview, int creativityReview, int replayabilityReview)
        {
            if (_data.Games.Any(game => game.IGDBGameId == gameId))
                return false;

            Game newGame = new Game()
            {
                UserId = userId,
                IGDBGameId = gameId,
                GameplayReview = gameplayReview,
                VisualReview = visualReview,
                PerformanceReview = performanceReview,
                AudioReview = audioReview,
                ExperienceReview = experienceReview,
                AtmosphereReview = atmosphereReview,
                CreativityReview = creativityReview,
                ReplayabilityReview = replayabilityReview
            };

            _data.Games.Add(newGame);
            _repository.SaveData(_data);
            return true;
        }
    }

}
