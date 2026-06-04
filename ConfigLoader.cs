using System.Text.Json;

public static class ConfigLoader
{
    public static T Load<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                $"Config file tidak ditemukan: {filePath}");
        }

        string json = File.ReadAllText(filePath);

        T? result = JsonSerializer.Deserialize<T>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (result == null)
        {
            throw new Exception(
                $"Gagal membaca konfigurasi: {filePath}");
        }

        return result;
    }
}