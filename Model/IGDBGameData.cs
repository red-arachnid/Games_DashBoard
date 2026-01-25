using System.Text.Json.Serialization;

namespace Games_DashBoard.Model
{
    public record IGDBGameData
    {
        [property: JsonPropertyName("id")] public int Id { get; set; }
        [property: JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
        [property: JsonPropertyName("first_release_date")] public long ReleaseDate { get; set; }
        [property: JsonPropertyName("summary")] public string Summary { get; set; } = string.Empty;
        [property: JsonPropertyName("game_modes")] public List<Info> GameModes { get; set; } = new List<Info>();
        [property: JsonPropertyName("genres")] public List<Info> Genres { get; set; } = new List<Info>();
        [property: JsonPropertyName("player_perspectives")] public List<Info> PlayerPerspectives { get; set; } = new List<Info>();
        [property: JsonPropertyName("themes")] public List<Info> Themes { get; set; } = new List<Info>();
    }

    public record Info
    {
        [property: JsonPropertyName("id")] public int Id { get; set; }
        [property: JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    }
}
