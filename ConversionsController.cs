
using Microsoft.AspNetCore.Mvc;
using UnitConversionApi.Models;
using UnitConversionApi.Services;

namespace UnitConversionApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversionsController : ControllerBase
{
    private readonly IUnitConversionService _conversionService;

    public ConversionsController(
        IUnitConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    [HttpGet("categories")]
    public IActionResult GetCategories()
    {
        return Ok(_conversionService.GetCategories());
    }

    [HttpGet("units/{category}")]
    public IActionResult GetUnits(string category)
    {
        try
        {
            return Ok(_conversionService.GetUnits(category));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("convert")]
    public IActionResult Convert([FromBody] ConversionRequest request)
    {
        try
        {
            var response = _conversionService.Convert(request);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
