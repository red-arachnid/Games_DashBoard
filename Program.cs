using Games_DashBoard.Data;
using Games_DashBoard.Model;
using Games_DashBoard.Services;
using Games_DashBoard.UI;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Games_DashBoard
{
    public class Program
    {
        static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            AnsiConsole.Profile.Capabilities.Ansi = true;
            AnsiConsole.Profile.Capabilities.Unicode = true;
            AnsiConsole.Clear();

            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            string clientId = config["IGDB:ClientId"];
            string token = config["IGDB:Token"];

            Repository repository = new Repository();
            StoredData data = await repository.LoadData();
            UserService userService = new UserService(repository, data);
            GameService gameService = new GameService(repository, data);
            IGDBService igdbService = new IGDBService(clientId, token);
            LoginScreenUI loginScreen = new LoginScreenUI(userService);
            MainScreenUI mainScreen = new MainScreenUI(gameService, igdbService);
            User currentUser = null!;

            string text = "Welcome! ";
            ClearConsole();

            while (true)
            {
                if (currentUser == null)
                {
                    var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title($"[grey]{text}Please select an option : [/]")
                        .AddChoices("Login", "Register", "Exit")
                        .HighlightStyle(new Style(Color.Gold1, decoration: Decoration.Bold)));

                    switch (choice)
                    {
                        case "Login":
                            currentUser = loginScreen.Login();
                            break;
                        case "Register":
                            await loginScreen.Register();
                            break;
                        case "Exit":
                            Environment.Exit(0);
                            break;
                    }
                    text = "";
                }
                else
                {
                    ClearConsole();
                    var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title($"[grey]Please select an option[/]")
                        .AddChoices("See Library", "Add New Game", "Log Out")
                        .HighlightStyle(new Style(Color.Gold1, decoration: Decoration.Bold)));

                    switch (choice)
                    {
                        case "See Library":
                            await mainScreen.ShowLibrary(currentUser);
                            break;
                        case "Add New Game":
                            await mainScreen.AddNewGame(currentUser);
                            break;
                        case "Log Out":
                            currentUser = null!;
                            break;
                    }
                }
            }
        }

        public static void ClearConsole()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Game Dashboard") { Color = Color.OrangeRed1, Justification = Justify.Center });
        }
    }
}
