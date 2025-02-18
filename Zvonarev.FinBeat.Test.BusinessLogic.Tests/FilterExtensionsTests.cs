using Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries.Query;
using Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries.Query.Internals;
using Zvonarev.FinBeat.Test.DomainObjects;

namespace Zvonarev.FinBeat.Test.BusinessLogic.Tests;

[TestClass]
public class FilterExtensionsTests
{
    private IEnumerable<OrderedDataEntry> GetTestCollection(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var n = i + 1;
            yield return new OrderedDataEntry(n, $"Value{n}", i);
        }
    }

    [TestMethod]
    public void ToPredicateTest1()
    {
        var codeToFind = 2;
        var filters = new[] { new CodePositiveFilter([codeToFind]) };

        var entriesCollection = GetTestCollection(3);

        var predicateExpr = filters.ToPredicate();
        var predicate = predicateExpr.Compile();
        var filteredResult = entriesCollection
            .Where(predicate)
            .ToArray();

        Assert.AreEqual(1, filteredResult.Length);

        var entry = filteredResult[0];
        Assert.AreEqual(codeToFind, entry.Code);
        Assert.AreEqual($"Value{codeToFind}", entry.Value);
    }

    [TestMethod]
    public void ToPredicateTest2()
    {
        var valueToFind = "Value1";
        var filters = new[] { new ValuePositiveFilter([valueToFind]) };

        var entriesCollection = GetTestCollection(3);

        var predicateExpr = filters.ToPredicate();
        var predicate = predicateExpr.Compile();
        var filteredResult = entriesCollection
            .Where(predicate)
            .ToArray();

        Assert.AreEqual(1, filteredResult.Length);

        var entry = filteredResult[0];
        Assert.AreEqual(1, entry.Code);
        Assert.AreEqual(valueToFind, entry.Value);
    }

    [TestMethod]
    public void ToPredicateTest3()
    {
        var codeNotToFind = 2;
        var filters = new[] { new CodeNegativeFilter([codeNotToFind]) };

        var entriesCollection = GetTestCollection(3);

        var predicateExpr = filters.ToPredicate();
        var predicate = predicateExpr.Compile();
        var filteredResult = entriesCollection
            .Where(predicate)
            .ToArray();

        Assert.AreEqual(2, filteredResult.Length);

        Assert.IsTrue(filteredResult.All(x => x.Code != codeNotToFind));
        Assert.IsTrue(filteredResult.Any(x => x.Code == 1));
        Assert.IsTrue(filteredResult.Any(x => x.Code == 3));
    }

    [TestMethod]
    public void ToPredicateTest4()
    {
        var valueNotToFind = "Value3";
        var filters = new[] { new ValueNegativeFilter([valueNotToFind]) };

        var entriesCollection = GetTestCollection(3);

        var predicateExpr = filters.ToPredicate();
        var predicate = predicateExpr.Compile();
        var filteredResult = entriesCollection
            .Where(predicate)
            .ToArray();

        Assert.AreEqual(2, filteredResult.Length);

        Assert.IsTrue(filteredResult.All(x => x.Value != valueNotToFind));
        Assert.IsTrue(filteredResult.Any(x => x.Value == "Value1"));
        Assert.IsTrue(filteredResult.Any(x => x.Value == "Value2"));
    }

    [TestMethod]
    public void ToPredicateTest5()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            var filters = Array.Empty<EntriesBaseFilter>();
            filters.ToPredicate();
        });
    }

    [TestMethod]
    public void ToPredicateTest6()
    {
        var codeNotToFind = 2;
        var valueNotToFind = "Value3";
        var filters = new EntriesBaseFilter[]
        {
            new ValueNegativeFilter([valueNotToFind]) ,
            new CodeNegativeFilter([codeNotToFind])
        };

        var entriesCollection = GetTestCollection(3);

        var predicateExpr = filters.ToPredicate();
        var predicate = predicateExpr.Compile();
        var filteredResult = entriesCollection
            .Where(predicate)
            .ToArray();

        Assert.AreEqual(1, filteredResult.Length);

        Assert.IsTrue(filteredResult.All(x => x.Code != codeNotToFind));
        Assert.IsTrue(filteredResult.All(x => x.Value != valueNotToFind));
        Assert.IsTrue(filteredResult.Any(x => x.Value == "Value1"));
    }

    [TestMethod]
    public void ToPredicateTest7()
    {
        var codeToFind = 2;
        var valueToFind = "Value3";
        var filters = new EntriesBaseFilter[]
        {
            new ValuePositiveFilter([valueToFind]) ,
            new CodePositiveFilter([codeToFind])
        };

        var entriesCollection = GetTestCollection(3);

        var predicateExpr = filters.ToPredicate();
        var predicate = predicateExpr.Compile();
        var filteredResult = entriesCollection
            .Where(predicate)
            .ToArray();

        Assert.AreEqual(0, filteredResult.Length);
    }

    [TestMethod]
    public void ToPredicateTest8()
    {
        var codeToFind = 3;
        var valueToFind = "Value3";
        var filters = new EntriesBaseFilter[]
        {
            new ValuePositiveFilter([valueToFind]) ,
            new CodePositiveFilter([codeToFind])
        };

        var entriesCollection = GetTestCollection(3);

        var predicateExpr = filters.ToPredicate();
        var predicate = predicateExpr.Compile();
        var filteredResult = entriesCollection
            .Where(predicate)
            .ToArray();

        Assert.AreEqual(1, filteredResult.Length);

        Assert.IsTrue(filteredResult.All(x => x.Code == codeToFind));
        Assert.IsTrue(filteredResult.All(x => x.Value == valueToFind));
    }

    [TestMethod]
    public void ToPredicateTest9()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            var filters = new EntriesBaseFilter[]
            {
                new ValuePositiveFilter(Array.Empty<string>()) ,
                new CodePositiveFilter(Array.Empty<int>())
            };

            filters.ToPredicate();
        });
    }
}
