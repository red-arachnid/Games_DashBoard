using System.Text.Json.Serialization;

namespace Games_DashBoard.Model
{
    public record IGDBGameData
    {
        [property: JsonPropertyName("id")] public int Id { get; set; }
        [property: JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
        [property: JsonPropertyName("first_release_date")] public long ReleaseDate { get; set; }
        [property: JsonPropertyName("summary")] public string Summary { get; set; } = string.Empty;
        [property: JsonPropertyName("involved_companies")] public List<CompanyInfo> InvolvedCompanies { get; set; } = new List<CompanyInfo>();
        [property: JsonPropertyName("genres")] public List<Info> Genres { get; set; } = new List<Info>();
        [property: JsonPropertyName("dlcs")] public List<Info> DLCs { get; set; } = new List<Info>();
        [property: JsonPropertyName("expansions")] public List<Info> Expansions { get; set; } = new List<Info>();
    }

    public record Info
    {
        [property: JsonPropertyName("id")] public int Id { get; set; }
        [property: JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    }
    public record CompanyInfo
    {
        [property: JsonPropertyName("id")] public int Id {  set; get; }
        [property: JsonPropertyName("company")] public int CompanyId { set; get; }
        [property: JsonPropertyName("developer")] public bool IsDeveloper { get; set; }
    }
}
