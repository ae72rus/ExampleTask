namespace Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries.Query;

public record CodeNegativeFilter(IReadOnlyCollection<int> Codes)
    : EntriesBaseFilter<int>(x => x.Code, [], Codes);