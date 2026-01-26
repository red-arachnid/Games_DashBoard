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
