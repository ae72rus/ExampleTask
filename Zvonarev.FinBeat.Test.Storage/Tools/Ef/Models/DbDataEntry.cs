using Zvonarev.FinBeat.Test.DomainObjects;

namespace Zvonarev.FinBeat.Test.Storage.Tools.Ef.Models;

internal record DbDataEntry(
    int Id,
    int OrderId,
    int Code,
    string Value
)
    : OrderedDataEntry(Code, Value, OrderId);