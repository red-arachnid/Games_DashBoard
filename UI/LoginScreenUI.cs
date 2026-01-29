using Games_DashBoard.Model;
using Games_DashBoard.Services;
using Spectre.Console;

namespace Games_DashBoard.UI
{
    public class LoginScreenUI
    {
        private UserService _userService;

        public LoginScreenUI(UserService userService)
        {
            _userService = userService;
        }

        public User Login()
        {
            User currentUser = null!;
            string username = AnsiConsole.Ask<string>("Enter [green]Username[/] : ");
            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]Password[/] : ")
                .PromptStyle("red")
                .Secret());

            AnsiConsole.Status().Start("Verifying Username & Password...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Aesthetic);
                Thread.Sleep(1000);
                currentUser = _userService.Login(username, password);
            });

            if (currentUser == null)
                AnsiConsole.MarkupLine("[bold red]Invalid Username or Password![/]");
            else
                AnsiConsole.MarkupLineInterpolated($"[green]Successfully logged in as {currentUser.Username}![/]");

            return currentUser!;
        }

        public async Task Register()
        {
            string username, password, confirmPassword;

            username = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter a [green]Username[/] : ")
                .Validate(name => _userService.CheckDuplicateUser(name)
                ? ValidationResult.Error("[red]Username already exists. Try another.[/]")
                : ValidationResult.Success()));

            password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter a [green]Password[/] : ")
                .PromptStyle("red")
                .Secret());
            confirmPassword = AnsiConsole.Prompt(
                new TextPrompt<string>("Confirm your [green]Password[/] : ")
                .PromptStyle("red")
                .Secret());

            if (String.Compare(password, confirmPassword, false) != 0)
            {
                AnsiConsole.MarkupLine("[red]The Password does not match![/]");
                return;
            }

            var confirm = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Are you sure you want to create this account?")
                .AddChoices("Yes", "No")
                .HighlightStyle(new Style(Color.Gold1, decoration: Decoration.Bold)));

            switch (confirm)
            {
                case "Yes":
                    bool isSuccess = await _userService.RegisterUser(username, password);
                    if (isSuccess)
                        AnsiConsole.MarkupLine("[green]Account created successfully! Please Login.[/]");
                    else
                        AnsiConsole.MarkupLine("[red]Error in account registration. Please try again..[/]");
                    break;
                case "No":
                    AnsiConsole.MarkupLine("[red]Aborting account registration![/]");
                    break;
            }
        }
    }
}
