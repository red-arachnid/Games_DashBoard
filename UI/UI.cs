using Games_DashBoard.Data;
using Games_DashBoard.Model;
using Games_DashBoard.Services;
using Spectre.Console;
using System.Text;

namespace Games_DashBoard.UI
{
    public class UI
    {
        private Repository _repository;
        private UserService _userService;
        private GameService _gameService;
        private IGDBService _igdbService;
        private User? _currentUser;

        private LoginScreen loginScreen;

        public UI()
        {
            _repository = new Repository();
            _userService = new UserService(_repository);
            _gameService = new GameService(_repository);

            loginScreen = new LoginScreen(_userService);
        }

        public void Start()
        {
            Console.OutputEncoding = Encoding.UTF8;
            AnsiConsole.Profile.Capabilities.Ansi = true;
            AnsiConsole.Profile.Capabilities.Unicode = true;
            AnsiConsole.Clear();

            AnsiConsole.Write(new FigletText("Game Dashboard") { Color = Color.OrangeRed1, Justification = Justify.Center });

            while (_currentUser == null)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Welcome! Please select an option : ")
                    .AddChoices("Login", "Register", "Exit"));

                switch (choice)
                {
                    case "Login":
                        loginScreen.Login(_currentUser!);
                        break;
                    case "Register":
                        loginScreen.Register();
                        break;
                    case "Exit":
                        Environment.Exit(0);
                        break;
                }
            }
        }

        
    }
}
