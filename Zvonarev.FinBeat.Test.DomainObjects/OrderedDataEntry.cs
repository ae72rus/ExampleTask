namespace Zvonarev.FinBeat.Test.DomainObjects;

public record OrderedDataEntry(int Code, string Value, int OrderId) : DataEntry(Code, Value);