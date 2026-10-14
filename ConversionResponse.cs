
namespace UnitConversionApi.Models;

public class ConversionResponse
{
    public string Category { get; set; } = string.Empty;

    public double OriginalValue { get; set; }

    public string FromUnit { get; set; } = string.Empty;

    public double Result { get; set; }

    public string ToUnit { get; set; } = string.Empty;
}
