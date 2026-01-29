using Games_DashBoard.Data;
using Games_DashBoard.Model;

namespace Games_DashBoard.Services
{
    public class UserService
    {
        private Repository _repository;
        private StoredData _data;

        public UserService(Repository repository, StoredData data)
        {
            _repository = repository;
            _data = data;
        }

        /// <summary>Login an existing user</summary>
        /// <returns>Returns the logged in user</returns>
        public User Login(string username, string password) 
        {
            //Find User
            var user = _data.Users.SingleOrDefault(u => u.Username.Equals(username, StringComparison.Ordinal));

            if (user == null)
                return null!;

            //Check Password
            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                return user;

            return null!;
        }

        /// <summary>Register a new user</summary>
        /// <returns>Returns true on successfull register else returns false</returns>
        public async Task<bool> RegisterUser(string username, string password)
        {
            if (CheckDuplicateUser(username))
                return false;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            User newUser = new User { Username = username, Password = hashedPassword };
            _data.Users.Add(newUser);
            await _repository.SaveData(_data);
            return true;
        }

        /// <summary>Checks if a user already exists with the given username</summary>
        /// <returns>Returns true if the user already exists else returns false</returns>
        public bool CheckDuplicateUser(string username)
        {
            if (_data.Users.Any(u => u.Username.Equals(username, StringComparison.Ordinal)))
                return true;

            return false;
        }
    }
}
