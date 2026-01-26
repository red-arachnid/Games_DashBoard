using Games_DashBoard.Model;
using Games_DashBoard.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Games_DashBoard
{
    public class Program
    {
        
        private static readonly string TOKEN = "9gu5yh01iufgzcoscmks8jhis59lu9";

        static async Task Main()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            string clientId = config["IGDB:ClientId"];

            IGDBService igdbService = new IGDBService(clientId, TOKEN);

            /*
            string gameName = "God of War";
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.igdb.com/v4/");
            client.DefaultRequestHeaders.Add("Client-ID", clientId);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {TOKEN}");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var query = "fields name, expansions.name, dlcs.name, first_release_date, game_modes.name, genres.name, player_perspectives.name, summary, themes.name;" 
                + $"search \"{gameName}\";"
                + "limit 5;";
            var content = new StringContent(query, Encoding.UTF8, "text/plain");

            var response = await client.PostAsync("games", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var datas = JsonSerializer.Deserialize<List<IGDBGameData>>(result);

                Console.WriteLine(datas[0].Id);
                Console.WriteLine(datas[0].Name);
                Console.WriteLine($"Release Date : {DateTimeOffset.FromUnixTimeSeconds(datas[0].ReleaseDate).DateTime}");

                Console.Write("Genres : ");
                foreach(var data in datas[0].GameModes)
                    Console.Write($"{data.Name},  ");
                foreach(var data in datas[0].Genres)
                    Console.Write($"{data.Name},  ");
                foreach(var data in datas[0].PlayerPerspectives)
                    Console.Write($"{data.Name},  ");
                foreach(var data in datas[0].Themes)
                    Console.Write($"{data.Name},  ");

                Console.WriteLine("\n\n");
                Console.WriteLine(datas[0].Summary);
            }
            else
            {
                Console.WriteLine("MEHHHH");
            }

            */
        }

        private static async Task<string> GetAccessToken(string clientId, string secret)
        {
            var client = new HttpClient();

            string authTokenUri = "https://id.twitch.tv/oauth2/token?"
                + $"client_id={clientId}&"
                + $"client_secret={secret}&"
                + $"grant_type=client_credentials";

            var response = await client.PostAsync(authTokenUri, null);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AccessToken>(content);
                return result?.Token ?? null!;
            }
            else return null!;
        }
    }

    public class AccessToken
    {
        [JsonPropertyName("access_token")] public string Token { get; set; } = string.Empty;
        [JsonPropertyName("expires_in")] public int ExpireIn { get; set; }
    }
}
