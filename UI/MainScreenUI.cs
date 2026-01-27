using Games_DashBoard.Model;
using Games_DashBoard.Services;
using Spectre.Console;

namespace Games_DashBoard.UI
{
    public class MainScreenUI
    {
        private UserService _userService;
        private GameService _gameService;
        private IGDBService _igdbService;

        public MainScreenUI(UserService userService, GameService gameService, IGDBService igdbService)
        {
            _userService = userService;
            _gameService = gameService;
            _igdbService = igdbService;
            Table table = new Table();
        }

        public async Task AddNewGame()
        {
            //Fetch Game
            string gameName = AnsiConsole.Prompt(
                new TextPrompt<string>("Search [green]Game[/] : "));

            List<IGDBGameData> games = await _igdbService.GetGamesByName(gameName);

            var gameNames = games.Select(game => game.Name).Append("Go Back").ToArray();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Result games : ")
                .AddChoices(gameNames));

            if (choice == "Go Back")
                return;

            //Separate Game Data
            IGDBGameData gameData = games.FirstOrDefault(game => game.Name == choice)!;
            if (gameData == null)
            {
                //Add logic for unable to fetch data
                return;
            }

            await ShowGameInfo(gameData);

            choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[grey]Select an action : [/]")
                .PageSize(3)
                .HighlightStyle(new Style(Color.Gold1, decoration: Decoration.Bold))
                .AddChoices("Add Game", "Go Back"));

            switch (choice)
            {
                case "Add Game":
                    //Ask for Review first
                    break;
                case "Go Back":
                    return;
            }
        }

        public async Task ShowLibrary(User currentUser)
        {
            List<Game> games = _gameService.GetLibraryOfUser(currentUser.Id);
            if (games == null) return;

            List<IGDBGameData> gamesData = await _igdbService.GetGamesByIds(games.Select(game => game.IGDBGameId).ToArray());
            if (gamesData == null) return;

            Table table = new Table();
            table.AddColumn("Serial No.");
            table.AddColumn("Game Name");

            int i = 1;
            foreach (IGDBGameData gameData in gamesData)
            {
                table.AddRow(i++.ToString(), gameData.Name);
            }
        }

        private async Task ShowGameInfo(IGDBGameData gameData)
        {
            string developer = await _igdbService.GetCompanyNameByCompanyId(gameData.InvolvedCompanies.FirstOrDefault(company => company.IsDeveloper)!.CompanyId);
            string releaseDate = DateTimeOffset.FromUnixTimeSeconds(gameData.ReleaseDate).ToString("yyyy/MM/dd");
            List<string> genres = gameData.Genres.Select(genre => genre.Name).ToList();
            List<string> dlcs = gameData.DLCs.Select(dl => dl.Name).ToList();
            dlcs.AddRange(gameData.Expansions.Select(exp => exp.Name).ToList());

            AnsiConsole.Clear();
            var header = new Padder(
                new Rows(
                    new Text(gameData.Name, new Style(Color.Gold1, decoration: Decoration.Bold)).Centered(),
                    new Text($"{developer} • {releaseDate}", new Style(Color.Grey)).Centered()))
                .Padding(0, 1, 0, 1);

            var gameInfoTable = new Table().NoBorder().HideHeaders().Centered();
            gameInfoTable.AddColumn("Left");
            if (genres.Count > 0)
                gameInfoTable.AddRow(new Markup($"[bold underline]Genres[/]\n{string.Join(", ", genres)}").Centered());

            gameInfoTable.AddEmptyRow();

            if (dlcs.Count > 0)
                gameInfoTable.AddRow(new Markup($"[bold underline]DLCs[/]\n{string.Join(", ", dlcs)}")).Centered();

            var layout = new Rows(
                header,
                new Rule().RuleStyle("grey").Centered(),
                new Padder(new Text(gameData.Summary).Centered()).Padding(4, 1, 4, 1),
                new Rule().RuleStyle("grey").Centered(),
                gameInfoTable);

            var panel = new Panel(layout)
                .Header(" Game Profile ")
                .Expand()
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.DeepSkyBlue1);

            AnsiConsole.Write(new Align(panel, HorizontalAlignment.Center, VerticalAlignment.Middle));
        }

        private void ReviewQuery()
        {

        }
    }
}
