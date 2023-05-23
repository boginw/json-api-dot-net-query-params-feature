using System.Linq.Expressions;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Controllers.Annotations;
using JsonApiDotNetCore.Queries;
using JsonApiDotNetCore.Queries.Internal.QueryableBuilding;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetExample.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JsonApiDotNetExample.Controllers;

[DisableRoutingConvention, Route("weather-forecast")]
public class WeatherForecastController : BaseJsonApiController<WeatherForecast, int?>
{
    private readonly IResourceGraph _resourceGraph;
    private readonly IResourceFactory _resourceFactory;
    private readonly IQueryLayerComposer _queryLayerComposer;

    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IJsonApiOptions options,
        IResourceGraph resourceGraph,
        ILoggerFactory loggerFactory,
        IResourceFactory resourceFactory,
        IQueryLayerComposer queryLayerComposer
    ) : base(options, resourceGraph, loggerFactory, getAll: null)
    {
        _resourceGraph = resourceGraph;
        _resourceFactory = resourceFactory;
        _queryLayerComposer = queryLayerComposer;
    }

    [HttpPost]
    public override Task<IActionResult> PostAsync(WeatherForecast resource, CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(Created("fake-url", resource));
    }

    [HttpGet]
    public override Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Wrapper = new Wrapper(Random.Shared.Next().ToString(), Random.Shared.Next().ToString())
            })
            .ToArray();

        var resourceType = _resourceGraph.GetResourceType<WeatherForecast>();
        var queryLayer = _queryLayerComposer.ComposeFromConstraints(resourceType);
        
        var builder = new QueryableBuilder(
            source: Expression.Constant(weatherForecasts),
            elementType: typeof(WeatherForecast),
            extensionType: typeof(Enumerable),
            nameFactory: new LambdaParameterNameFactory(),
            resourceFactory: _resourceFactory,
            entityModel: new RuntimeModel()
        );

        var lambdaExpression = Expression.Lambda(builder.ApplyQuery(queryLayer));
        var results = (IEnumerable<WeatherForecast>)lambdaExpression.Compile().DynamicInvoke()!;

        return Task.FromResult<IActionResult>(Ok(results.ToArray()));
    }
}