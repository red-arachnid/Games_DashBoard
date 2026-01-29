using Games_DashBoard.Model;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Games_DashBoard.Data
{
    public class Repository
    {
        private readonly string FILE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");

        public async Task<StoredData> LoadData()
        {
            if (!File.Exists(FILE_PATH)) 
                return new StoredData();

            try
            {
                string jsonString = await File.ReadAllTextAsync(FILE_PATH);
                var data = JsonSerializer.Deserialize<StoredData>(jsonString);
                return data ?? new StoredData();
            }   
            catch (JsonException)
            {
                return new StoredData();
            }
        }

        public async Task SaveData(StoredData data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(data, options);
            await File.WriteAllTextAsync(FILE_PATH, jsonString);
        }
    }
}
