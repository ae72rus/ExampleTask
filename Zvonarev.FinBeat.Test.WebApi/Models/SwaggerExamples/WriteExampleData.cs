using Swashbuckle.AspNetCore.Filters;

namespace Zvonarev.FinBeat.Test.WebApi.Models.SwaggerExamples;

public record WriteExampleData : IExamplesProvider<Dictionary<int, string>[]>
{
    public Dictionary<int, string>[] GetExamples()
        =>
        [
            new()
            {
                {1, "value1"}
            },
            new()
            {
                {5, "value2"}
            },
            new()
            {
                {10, "value32"}
            }
        ];
}