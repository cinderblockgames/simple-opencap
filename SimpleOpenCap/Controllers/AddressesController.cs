using Microsoft.AspNetCore.Mvc;
using SimpleOpenCap.Data;

namespace SimpleOpenCap.Controllers;

[ApiController]
[Route("v1/addresses")]
[Produces("application/json")]
public class AddressesController : ControllerBase
{
    private DataReader Reader { get; }

    public AddressesController(DataReader reader)
    {
        Reader = reader;
    }

    [HttpGet(Name = "addresses")]
    public async Task<IActionResult> Get([FromQuery] string? alias, [FromQuery(Name = "address_type")] string? type)
    {
        try
        {
            var data = await Reader.ReadData();
            if (alias != null && data.ContainsKey(alias))
            {
                var addresses = data[alias];
                if (type != null)
                {
                    addresses = addresses.Where(info => string.Equals(info.AddressType, type, StringComparison.Ordinal));
                }

                if (addresses.Any())
                {
                    return new JsonResult(
                        addresses.Count() == 1 ? addresses.First() : addresses
                    );
                }
            }
        
            return new EmptyResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { message = "unexpected error" });
        }
    }
}