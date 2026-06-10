using System.Text.Json;

public static class ConfigLoader
{
    private static JsonSerializerOptions? _options;

    public static T Load<T>(string filePath)
    {
        // Normalisasi path untuk mencegah path traversal
        string fullPath = Path.GetFullPath(filePath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException(
                $"Config file tidak ditemukan.", fullPath); // Pesan dan path dipisah 

        string json = File.ReadAllText(fullPath);

        if (string.IsNullOrWhiteSpace(json))
            throw new InvalidOperationException(
                $"Config file kosong: {fullPath}");

        try
        {
            T result = JsonSerializer.Deserialize<T>(json, _options)
                ?? throw new InvalidOperationException(
                    $"Deserialisasi menghasilkan null untuk tipe {typeof(T).Name}.");

            return result;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"Format JSON tidak valid pada file: {fullPath}", ex);
        }
    }
}