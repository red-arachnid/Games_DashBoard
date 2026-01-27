using Games_DashBoard.Model;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Games_DashBoard.Services
{
    public class IGDBService
    {
        private HttpClient _client;

        public IGDBService(string clientId, string token)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://api.igdb.com/v4/");
            _client.DefaultRequestHeaders.Add("Client-ID", clientId);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>Search IGDB for the specified game</summary>
        /// <returns>Returns a list of games found</returns>
        public async Task<List<IGDBGameData>> GetGamesByName(string gameName)
        {
            string query = "fields name, expansions.name, dlcs.name, first_release_date, genres.name, summary, involved_companies.developer, involved_companies.company;"
                + $"search \"{gameName}\";"
                + "limit 10;";
            var content = new StringContent(query, Encoding.UTF8, "text/plain");
            var response = await _client.PostAsync("games", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var gameData = JsonSerializer.Deserialize<List<IGDBGameData>>(result);
                return gameData ?? null!;
            }
            else return null!;
        }

        /// <summary>Search IGDB for the game of specified Id</summary>
        /// <returns>Returns the game with that Id</returns>
        public async Task<IGDBGameData> GetGameById(int id)
        {
            string query = "fields name, expansions.name, dlcs.name, first_release_date, genres.name, summary, involved_companies.developer, involved_companies.company;"
                + $"where id = {id};"
                + "limit 5;";
            var content = new StringContent(query, Encoding.UTF8, "text/plain");
            var response = await _client.PostAsync("games", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var gameData = JsonSerializer.Deserialize<List<IGDBGameData>>(result);
                return gameData?[0] ?? null!;
            }
            else return null!;
        }

        /// <summary>Search IGDB for the games of specified Ids</summary>
        /// <returns>Returns a list of games with those ids</returns>
        public async Task<List<IGDBGameData>> GetGamesByIds(int[] ids)
        {
            string inputIds = $"({string.Join(",", ids)})";

            string query = "fields name, expansions.name, dlcs.name, first_release_date, genres.name, summary, involved_companies.developer, involved_companies.company;"
                + $"where id = {inputIds};"
                + "limit 5;";
            var content = new StringContent(query, Encoding.UTF8, "text/plain");
            var response = await _client.PostAsync("games", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var gameData = JsonSerializer.Deserialize<List<IGDBGameData>>(result);
                return gameData ?? null!;
            }
            else return null!;
        }

        /// <summary>Get Company Name from its company Id</summary>
        public async Task<string> GetCompanyNameByCompanyId(int companyId)
        {
            string query = $"fields name; where id = {companyId}; limit 1;";
            var content = new StringContent(query, Encoding.UTF8, "text/plain");
            var response = await _client.PostAsync("companies", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var gameData = JsonSerializer.Deserialize<List<IGDBCompany>>(result);
                return gameData?[0].Name ?? null!;
            }
            else return null!;
        }
        private record IGDBCompany ([property: JsonPropertyName("id")] int Id, [property: JsonPropertyName("name")] string Name);
    }
}
