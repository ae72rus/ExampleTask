namespace Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries.Query;

public record CodePositiveFilter(IReadOnlyCollection<int> Codes)
    : EntriesBaseFilter<int>(x => x.Code, Codes, []);