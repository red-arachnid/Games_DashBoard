using Games_DashBoard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Games_DashBoard.Data
{
    public class Repository
    {
        private readonly string FILE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");

        public StoredData LoadData()
        {
            if (!File.Exists(FILE_PATH)) 
                return new StoredData();

            try
            {
                string jsonString = File.ReadAllText(FILE_PATH);
                var data = JsonSerializer.Deserialize<StoredData>(jsonString);
                return data ?? new StoredData();
            }
            catch
            {
                return new StoredData();
            }
        }

        public void SaveData(StoredData data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(FILE_PATH, jsonString);
        }
    }
}
