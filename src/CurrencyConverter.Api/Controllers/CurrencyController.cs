using Microsoft.AspNetCore.Mvc;
using CurrencyConverter.Api.Models;

namespace CurrencyConverter.Api.Controllers;

[ApiController]
[Route("api/currency")]
public class CurrencyController : ControllerBase
{
    [HttpPost("convert")]
    public IActionResult Convert([FromBody] ConvertRequest request)
    {
        try
        {
            var converter = new CurrencyConverter.Core.CurrencyConverter();

            string result = converter.Convert(request.Amount, request.Language);

            return Ok(new { result });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}