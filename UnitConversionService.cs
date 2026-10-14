
using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

public class UnitConversionService : IUnitConversionService
{
    private readonly Dictionary<string, Dictionary<string, UnitDefinition>>
        _units = new(StringComparer.OrdinalIgnoreCase)
        {
            ["length"] = new Dictionary<string, UnitDefinition>(
            StringComparer.OrdinalIgnoreCase)
            {
                ["meter"] = new() { Name = "Meter", Symbol = "m", FactorToBase = 1 },
                ["kilometer"] = new() { Name = "Kilometer", Symbol = "km", FactorToBase = 1000 },
                ["centimeter"] = new() { Name = "Centimeter", Symbol = "cm", FactorToBase = 0.01 },
                ["millimeter"] = new() { Name = "Millimeter", Symbol = "mm", FactorToBase = 0.001 },
                ["foot"] = new() { Name = "Foot", Symbol = "ft", FactorToBase = 0.3048 },
                ["inch"] = new() { Name = "Inch", Symbol = "in", FactorToBase = 0.0254 },
                ["mile"] = new() { Name = "Mile", Symbol = "mi", FactorToBase = 1609.344 },
                ["yard"] = new() { Name = "Yard", Symbol = "yd", FactorToBase = 0.9144 }
            },

            ["mass"] = new Dictionary<string, UnitDefinition>(
            StringComparer.OrdinalIgnoreCase)
            {
                ["kilogram"] = new() { Name = "Kilogram", Symbol = "kg", FactorToBase = 1 },
                ["gram"] = new() { Name = "Gram", Symbol = "g", FactorToBase = 0.001 },
                ["milligram"] = new() { Name = "Milligram", Symbol = "mg", FactorToBase = 0.000001 },
                ["pound"] = new() { Name = "Pound", Symbol = "lb", FactorToBase = 0.45359237 },
                ["ounce"] = new() { Name = "Ounce", Symbol = "oz", FactorToBase = 0.028349523125 },
                ["tonne"] = new() { Name = "Metric Tonne", Symbol = "t", FactorToBase = 1000 }
            },

            ["temperature"] = new Dictionary<string, UnitDefinition>(
            StringComparer.OrdinalIgnoreCase)
            {
                ["celsius"] = new() { Name = "Celsius", Symbol = "°C", FactorToBase = 1 },
                ["fahrenheit"] = new() { Name = "Fahrenheit", Symbol = "°F", FactorToBase = 1 },
                ["kelvin"] = new() { Name = "Kelvin", Symbol = "K", FactorToBase = 1 }
            }
        };

    public IEnumerable<string> GetCategories()
    {
        return _units.Keys.OrderBy(x => x);
    }

    public IEnumerable<UnitDefinition> GetUnits(string category)
    {
        if (!_units.TryGetValue(category, out var units))
        {
            throw new ArgumentException(
                $"Unknown category '{category}'. Supported categories: " +
                string.Join(", ", GetCategories()));
        }

        return units.Values.OrderBy(x => x.Name);
    }

    public ConversionResponse Convert(ConversionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Category))
            throw new ArgumentException("Category is required.");

        if (string.IsNullOrWhiteSpace(request.FromUnit))
            throw new ArgumentException("FromUnit is required.");

        if (string.IsNullOrWhiteSpace(request.ToUnit))
            throw new ArgumentException("ToUnit is required.");

        if (double.IsNaN(request.Value) ||
            double.IsInfinity(request.Value))
        {
            throw new ArgumentException("Value must be a finite number.");
        }

        if (!_units.TryGetValue(request.Category, out var units))
        {
            throw new ArgumentException(
                $"Unsupported category '{request.Category}'.");
        }

        if (!units.TryGetValue(request.FromUnit, out var from))
        {
            throw new ArgumentException(
                $"Unsupported source unit '{request.FromUnit}'.");
        }

        if (!units.TryGetValue(request.ToUnit, out var to))
        {
            throw new ArgumentException(
                $"Unsupported target unit '{request.ToUnit}'.");
        }

        double result;

        if (request.Category.Equals(
            "temperature", StringComparison.OrdinalIgnoreCase))
        {
            result = ConvertTemperature(
                request.Value, request.FromUnit, request.ToUnit);
        }
        else
        {
            result = request.Value * from.FactorToBase
                     / to.FactorToBase;
        }

        if (double.IsNaN(result) || double.IsInfinity(result))
        {
            throw new ArgumentException(
                "The converted result is outside the supported numeric range.");
        }

        return new ConversionResponse
        {
            Category = request.Category.ToLowerInvariant(),
            OriginalValue = request.Value,
            FromUnit = from.Symbol,
            Result = result,
            ToUnit = to.Symbol
        };
    }

    private static double ConvertTemperature(
        double value, string fromUnit, string toUnit)
    {
        double celsius = fromUnit.ToLowerInvariant() switch
        {
            "celsius" => value,
            "fahrenheit" => (value - 32) * 5 / 9,
            "kelvin" => value - 273.15,
            _ => throw new ArgumentException("Unsupported temperature unit.")
        };

        if (celsius < -273.15)
            throw new ArgumentException(
                "Temperature cannot be below absolute zero.");

        return toUnit.ToLowerInvariant() switch
        {
            "celsius" => celsius,
            "fahrenheit" => celsius * 9 / 5 + 32,
            "kelvin" => celsius + 273.15,
            _ => throw new ArgumentException("Unsupported temperature unit.")
        };
    }
}
