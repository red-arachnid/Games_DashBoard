using Games_DashBoard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        public async Task<List<IGDBGameData>> GetGamesByName(string gameName)
        {
            string query = "fields name, expansions.name, dlcs.name, first_release_date, game_modes.name, genres.name, player_perspectives.name, summary, themes.name;"
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

        public async Task<IGDBGameData> GetGameById(int id)
        {
            string query = "fields name, expansions.name, dlcs.name, first_release_date, game_modes.name, genres.name, player_perspectives.name, summary, themes.name;"
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

        public async Task<List<IGDBGameData>> GetGamesByIds(int[] ids)
        {
            string inputIds = $"({string.Join(",", ids)})";

            string query = "fields name, expansions.name, dlcs.name, first_release_date, game_modes.name, genres.name, player_perspectives.name, summary, themes.name;"
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
    }
}
