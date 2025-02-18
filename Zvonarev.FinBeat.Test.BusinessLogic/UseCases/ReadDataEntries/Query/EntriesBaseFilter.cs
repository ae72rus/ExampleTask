using System.Linq.Expressions;
using Zvonarev.FinBeat.Test.DomainObjects;

namespace Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries.Query;

public abstract record EntriesBaseFilter
{
    internal Expression<Func<DataEntry, object?>> FilteredProperty { get; }
    internal IEnumerable<object?> AllowedValues { get; }
    internal IEnumerable<object?> ForbiddenValues { get; }

    internal EntriesBaseFilter(
        Expression<Func<DataEntry, object?>> filteredProperty,
        IEnumerable<object?> allowedValues,
        IEnumerable<object?> forbiddenValues
    )
    {
        FilteredProperty = filteredProperty;
        AllowedValues = allowedValues;
        ForbiddenValues = forbiddenValues;
    }

    protected static Expression<Func<DataEntry, object?>> GetGeneralizedExpression<T>(Expression<Func<DataEntry, T>> sourceExp)
    {
        var parameter = sourceExp.Parameters[0];
        var body = Expression.Convert(sourceExp.Body, typeof(object));
        return Expression.Lambda<Func<DataEntry, object?>>(body, parameter);
    }
}

public abstract record EntriesBaseFilter<T> : EntriesBaseFilter
{
    protected EntriesBaseFilter(
        Expression<Func<DataEntry, T>> filteredProperty,
        IEnumerable<T> allowedValues,
        IEnumerable<T> forbiddenValues
    )
        : base(
            GetGeneralizedExpression(filteredProperty),
            allowedValues.Select(x => x as object),//Cast<object> does not work with strings
            forbiddenValues.Select(x => x as object)
        )
    {
    }
}