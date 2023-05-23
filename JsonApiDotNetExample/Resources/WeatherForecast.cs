using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace JsonApiDotNetExample.Resources;

[Resource(GenerateControllerEndpoints = JsonApiEndpoints.None)]
public class WeatherForecast : IIdentifiable<int?>
{
    public int? Id { get; set; }

    public string? StringId
    {
        get => Id.ToString();
        set => Id = value == null ? null : int.Parse(value);
    }

    public string? LocalId { get; set; }

    [Attr]
    public DateOnly Date { get; set; }

    [Attr]
    public int TemperatureC { get; set; }
    
    [Attr]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    [Attr]
    public string? Summary { get; set; }

    [Attr]
    public Wrapper? Wrapper { get; set; }
}