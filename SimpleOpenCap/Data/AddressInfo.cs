using System.Dynamic;
using Newtonsoft.Json;

namespace SimpleOpenCap.Data;

public class AddressInfo
{
    
    [JsonProperty("address_type")]
    public string AddressType { get; set; }
    
    public string Address { get; set; }
    
    public ExpandoObject? Extensions { get; set; }
    
}