using System.Linq.Expressions;
using System.Reflection;
using Zvonarev.FinBeat.Test.DomainObjects;

namespace Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries.Query.Internals;

internal static class FilterExtensions
{
    public static Expression<Func<OrderedDataEntry, bool>> ToPredicate(this IReadOnlyCollection<EntriesBaseFilter> filters)
    {
        if (!filters.Any())
            throw new ArgumentException("Filters collection contains no elements", nameof(filters));

        if (!filters.SelectMany(x => x.AllowedValues).Any() && !filters.SelectMany(x => x.ForbiddenValues).Any())
            throw new ArgumentException("Filters contains no allowed values neither forbidden values", nameof(filters));

        Expression? resultExp = null;
        var paramExp = Expression.Parameter(typeof(OrderedDataEntry), "x");
        foreach (var filter in filters)
        {
            var propertyInfo = filter.GetPropertyInfo();
            var memberExp = Expression.Property(paramExp, propertyInfo);
            var allowedContainsExp = filter.AllowedValues.CallContainsOn(memberExp);
            var forbiddenContainsExp = filter.ForbiddenValues.CallContainsOn(memberExp);
            var negatedForbiddenExp = Expression.IsFalse(forbiddenContainsExp);

            Expression combinedFilterExp;
            if (filter.AllowedValues.Any() && filter.ForbiddenValues.Any())
                combinedFilterExp = Expression.OrElse(allowedContainsExp, negatedForbiddenExp);//allowed collection contains element OR forbidden collection does not contain element
            else if (filter.AllowedValues.Any())
                combinedFilterExp = allowedContainsExp;//allowed collection contains element
            else if (filter.ForbiddenValues.Any())
                combinedFilterExp = negatedForbiddenExp;//forbidden collection does not contain element
            else
                continue;//wtf cases

            resultExp = resultExp == null
            ? combinedFilterExp
            : Expression.AndAlso(resultExp, combinedFilterExp);
        }

        return Expression.Lambda<Func<OrderedDataEntry, bool>>(
                resultExp ?? throw new InvalidOperationException("Failed to get predicate from filters collection"),
                paramExp
            );
    }

    private static MethodInfo GetContainsMethod()
    {
        return typeof(Enumerable).GetMethods()
                   .FirstOrDefault(x =>
                       x.ContainsGenericParameters
                       && x.GetParameters().Length == 2
                       && x.Name == nameof(Enumerable.Contains))
                   ?.MakeGenericMethod(typeof(object))
               ?? throw new InvalidOperationException("Unable to find Enumerable.Contains method");
    }

    private static PropertyInfo GetPropertyInfo(this EntriesBaseFilter filter)
    {
        var expression = filter.FilteredProperty.Body;

        while (expression is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unaryExpression)
            expression = unaryExpression.Operand;

        return (expression as MemberExpression)?.Member as PropertyInfo
               ?? throw new InvalidOperationException($"Failed to get property info from {nameof(EntriesBaseFilter.FilteredProperty)}");
    }

    private static MethodCallExpression CallContainsOn(this IEnumerable<object> enumeration, MemberExpression memberExpression)
    {
        var iEnumerableContainsMethodInfo = GetContainsMethod();
        var @enum = Expression.Constant(enumeration);
        var convertedMember = Expression.Convert(memberExpression, typeof(object));
        var callContainsExp = Expression.Call(null, iEnumerableContainsMethodInfo, @enum, convertedMember);
        return callContainsExp;
    }
}