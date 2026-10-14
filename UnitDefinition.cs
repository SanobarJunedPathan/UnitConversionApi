
namespace UnitConversionApi.Models;

public class UnitDefinition
{
    public string Name { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public double FactorToBase { get; set; }
}
