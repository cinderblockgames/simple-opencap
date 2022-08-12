using Newtonsoft.Json;

namespace SimpleOpenCap.Data;

public class DataReader
{
    private string Path { get; } = "config/data.json";

    public async Task<IDictionary<string, IEnumerable<AddressInfo>>> ReadData()
    {
        var contents = await File.ReadAllTextAsync(Path);
        var data = JsonConvert.DeserializeObject<IDictionary<string, IEnumerable<AddressInfo>>>(contents);
        var dict = new Dictionary<string, IEnumerable<AddressInfo>>(data, StringComparer.OrdinalIgnoreCase);

        foreach (var address in dict.SelectMany(kvp => kvp.Value))
        {
            address.Extensions ??= new();
        }

        return dict;
    }
}