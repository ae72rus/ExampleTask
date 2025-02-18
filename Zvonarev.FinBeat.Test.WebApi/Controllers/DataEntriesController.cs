using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries;
using Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries.Query;
using Zvonarev.FinBeat.Test.BusinessLogic.UseCases.WriteDataEntries;
using Zvonarev.FinBeat.Test.DomainObjects;
using Zvonarev.FinBeat.Test.WebApi.Models;
using Zvonarev.FinBeat.Test.WebApi.Models.SwaggerExamples;

namespace Zvonarev.FinBeat.Test.WebApi.Controllers;

[ApiController]
[Route("api/v1/data")]
public class DataEntriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DataEntriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(Dictionary<int, string>[]), typeof(WriteExampleData))]
    public async Task<IActionResult> WriteDataEntries([FromBody] Dictionary<int, string>[]? data, CancellationToken ct)
    {
        if (data == null)
            return BadRequest(new ErrorResponse
            {
                ErrorMessage = "Data is required"
            });

        if (!data.Any())
            return BadRequest(new ErrorResponse
            {
                ErrorMessage = "Data array must contain items"
            });

        var command = new WriteDataEntriesCommand(
            data
                .SelectMany(x => x)
                .Select(x => new DataEntry(x.Key, x.Value)).ToArray()
        );

        await _mediator.Send(command, ct);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(DataEntryResponseModel[]))]
    public async Task<IActionResult> ReadDataEntries(
        CancellationToken ct,
        [FromQuery(Name = "codes")] int[]? requiredCodes = null,
        [FromQuery(Name = "values")] string[]? requiredValues = null
    )

    {
        var filters = new List<EntriesBaseFilter>();
        if (requiredCodes?.Any() == true)
            filters.Add(new CodePositiveFilter(requiredCodes));
        if (requiredValues?.Any() == true)
            filters.Add(new ValuePositiveFilter(requiredValues));

        var query = new EntriesQuery(filters);
        var entries = await _mediator.Send(query, ct);

        return Ok(entries.Select(x => new DataEntryResponseModel
        {
            Code = x.Code,
            Value = x.Value,
            OrderId = x.OrderId
        }));
    }
}