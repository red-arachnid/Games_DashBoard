using Games_DashBoard.Data;
using Games_DashBoard.Model;

namespace Games_DashBoard.Services
{
    public class GameService
    {
        private Repository _repository;
        private StoredData _data;

        public GameService(Repository repository)
        {
            _repository = repository;
            _data = _repository.LoadData();
        }


        /// <summary>Get the List of Game Library of a User</summary>
        public List<Game> GetLibraryOfUser(Guid userId)
            => _data.Games.Where(game => game.UserId == userId).ToList();

        /// <summary>Add a New Game To User Library</summary>
        /// <returns>Returns false if the game is already in user's library</returns>
        public bool AddNewGame(Game game)
        {
            if (_data.Games.Any(game => game.IGDBGameId == game.IGDBGameId))
                return false;

            _data.Games.Add(game);
            _repository.SaveData(_data);
            return true;
        }
    }

}
