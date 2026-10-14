
namespace UnitConversionApi.Models;

public class ConversionRequest
{
    public string Category { get; set; } = string.Empty;

    public double Value { get; set; }

    public string FromUnit { get; set; } = string.Empty;

    public string ToUnit { get; set; } = string.Empty;
}
