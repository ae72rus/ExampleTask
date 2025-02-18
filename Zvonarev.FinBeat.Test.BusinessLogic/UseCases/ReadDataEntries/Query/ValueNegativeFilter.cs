namespace Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries.Query;

public record ValueNegativeFilter(IReadOnlyCollection<string> Values)
    : EntriesBaseFilter<string>(x => x.Value, [], Values);