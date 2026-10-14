
using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

public interface IUnitConversionService
{
    IEnumerable<string> GetCategories();

    IEnumerable<UnitDefinition> GetUnits(string category);

    ConversionResponse Convert(ConversionRequest request);
}
