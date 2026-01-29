using Games_DashBoard.Model;
using Games_DashBoard.Services;
using Spectre.Console;
using Spectre.Console.Extensions;

namespace Games_DashBoard.UI
{
    public class MainScreenUI
    {
        private GameService _gameService;
        private IGDBService _igdbService;

        public MainScreenUI(GameService gameService, IGDBService igdbService)
        {
            _gameService = gameService;
            _igdbService = igdbService;
        }

        public async Task AddNewGame(User currentUser)
        {
            //Fetch Game
            string gameName = AnsiConsole.Prompt(
                new TextPrompt<string>("Search [green]Game[/] : "));

            List<IGDBGameData> games = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Aesthetic)
                .SpinnerStyle(Style.Parse("gold1"))
                .StartAsync("Searching Games....", async ctx => await _igdbService.GetGamesByName(gameName));

            var gameNames = games.Select(game => game.Name).Append("Go Back").ToArray();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Result games : ")
                .AddChoices(gameNames)
                .HighlightStyle(new Style(Color.Gold1, decoration: Decoration.Bold)));

            if (choice == "Go Back")
                return;

            //Separate Game Data
            IGDBGameData gameData = games.FirstOrDefault(game => game.Name == choice)!;
            if (gameData == null)
            {
                //Add logic for unable to fetch data
                AnsiConsole.MarkupLine("[red]Something went wrong. Please try again.");
                Console.ReadKey(true);
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
                    var reviewFields = new[] { "Gameplay", "Story", "Visuals", "Audio", "Creativity" };
                    var review = ReviewQuery(reviewFields);

                    choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("[grey]Are you sure you want to add this game to your library??[/]")
                        .PageSize(3)
                        .HighlightStyle(new Style(Color.Gold1, decoration: Decoration.Bold))
                        .AddChoices("Yes", "No"));

                    if (choice == "No") return;

                    bool isSuccess = await _gameService.AddNewGame(
                        currentUser.Id,
                        gameData.Id,
                        review[reviewFields[0]],
                        review[reviewFields[1]],
                        review[reviewFields[2]],
                        review[reviewFields[3]],
                        review[reviewFields[4]]
                    );

                    if (isSuccess)
                        AnsiConsole.MarkupLine($"[green]Successfully added {gameData.Name} to your Library[/]");
                    else
                        AnsiConsole.MarkupLine($"[red]There was a problem while adding the game. Please try again.[/]");
                    Console.ReadKey();

                    break;
                case "Go Back":
                    return;
            }
        }

        public async Task ShowLibrary(User currentUser)
        {
            List<Game> games = _gameService.GetLibraryOfUser(currentUser.Id);
            if (games == null) return;

            if (games.Count == 0)
            {
                AnsiConsole.Markup($"[red bold]No Games in your library![/]\n[grey]You can add games by going to add game option[/]");
                Console.ReadKey(true);
                return;
            }

            List<IGDBGameData> gamesData = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Aesthetic)
                .SpinnerStyle(Style.Parse("gold1"))
                .StartAsync("Fetching Game Library....", async ctx => await _igdbService.GetGamesByIds(games.Select(game => game.IGDBGameId).ToArray()));

            if (gamesData == null)
            {
                AnsiConsole.Markup($"[red bold]There was some problem while fetching your game data. Please try again[/]");
                Console.ReadKey(true);
                return;
            }
            
            //Loop if the user wants to show data in a different order 
            while (true)
            {
                Program.ClearConsole();
                var header = new Padder(
                new Rows(new Text("Your Game Library", new Style(Color.Gold1, decoration: Decoration.Bold)).Centered()))
                .Padding(0, 1, 0, 1);

                Table table = new Table().HideHeaders().NoBorder();
                table.AddColumn("Sr Number");
                table.AddColumn("Name").Width(70);
                table.AddColumn("Date Added");
                table.AddColumn("Rating");
                int i = 1;
                foreach (IGDBGameData gameData in gamesData)
                {
                    Game currentGame = games.FirstOrDefault(g => g.IGDBGameId == gameData.Id, null!);
                    double score = currentGame?.FinalScore ?? 0;
                    int starCount = (int)Math.Round(score);
                    string stars = new string('★', starCount);
                    string empty = new string('☆', 10 - starCount);
                    string ratingRender = $"[yellow]{stars}[/][grey]{empty}[/]";

                    string releaseDate = currentGame?.DateAdded.ToString("dd/MM/yyyy") ?? "";

                    table.AddRow(i++.ToString(), gameData.Name, releaseDate, ratingRender);
                }

                var layout = new Rows(
                    header,
                    new Rule().RuleStyle("grey").Centered(),
                    table);

                var panel = new Panel(layout)
                    .Header(" Game Profile ")
                    .Expand()
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.DeepSkyBlue1);

                AnsiConsole.Write(new Align(panel, HorizontalAlignment.Center, VerticalAlignment.Middle));

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[grey]Select your action : [/]")
                    .AddChoices("Go Back", "Sort By Name", "Sort By Date Added")
                    .HighlightStyle(new Style(Color.Gold1, decoration: Decoration.Bold)));

                switch (choice)
                {
                    case "Sort By Name":
                        gamesData = gamesData.OrderBy(data => data.Name).ToList();
                        break;
                    case "Sort By Date Added":
                        gamesData = gamesData.OrderBy(data => data.ReleaseDate).ToList();
                        break;
                    case "Go Back":
                        return;
                }
            }
        }

        private async Task ShowGameInfo(IGDBGameData gameData)
        {
            var devCompany = gameData.InvolvedCompanies.FirstOrDefault(company => company.IsDeveloper);

            string developer = (devCompany != null) 
                ? await _igdbService.GetCompanyNameByCompanyId(devCompany.CompanyId)
                : "Unknown Company";

            string releaseDate = DateTimeOffset.FromUnixTimeSeconds(gameData.ReleaseDate).ToString("yyyy/MM/dd");


            List<string> genres = gameData.Genres?.Select(genre => genre.Name).ToList() ?? new List<string>();
            List<string> dlcs = gameData.DLCs?.Select(dl => dl.Name).ToList() ?? new List<string>();
            if (gameData.Expansions != null)
                dlcs.AddRange(gameData.Expansions.Select(exp => exp.Name).ToList());

            Program.ClearConsole();
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

        private Dictionary<string, int> ReviewQuery(string[] reviewFields)
        {
            var reviews = new Dictionary<string, int>();

            var header = new Panel(
                new Text("GAME EVALUATION", new Style(Color.Gold1, decoration: Decoration.Bold)).Centered())
                .BorderColor(Color.DeepSkyBlue1)
                .Border(BoxBorder.Rounded)
                .Expand();

            Program.ClearConsole();
            AnsiConsole.Write(new Align(header, HorizontalAlignment.Center));

            foreach (var field in reviewFields)
            {
                var response = AnsiConsole.Prompt(
                    new TextPrompt<int>($"[bold underline]Rate {field} of the game [grey](between 0 - 10)[/]: [/]")
                    .PromptStyle("gold1")
                    .Validate(review => (review > 10 || review < 0)
                    ? ValidationResult.Error("[red]Please enter a valid review [grey](between 0 - 10)[/][/]")
                    : ValidationResult.Success()));

                reviews[field] = response;
                AnsiConsole.Write(new Rule().RuleStyle("grey").Centered());
            }
            return reviews;
        }
    }
}
