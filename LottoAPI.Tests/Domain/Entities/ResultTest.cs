using LottoAPI.Domain.Entities;

namespace LottoAPI.Test.Domain.Entities;

[TestFixture]
public class ResultTest
{
    [Test]
    public void CheckResult_ShouldReturnAPartialResult_WhenOnlyAFewNumberAreAHit()
    {
        var expectedHitNumbers = new List<int> { 4, 5 };
        var expectedHits = 2;

        var result = new Result
        {
            Id = 0,
            LottoId = string.Empty,
            Numbers = new List<int> { 1, 2, 3, 4, 5 },
        };

        var resultCheck = result.CheckResult(new List<int> { 4, 5, 6, 7, 8, 9 });

        Assert.That(resultCheck.Hits, Is.EqualTo(expectedHits));
        Assert.That(resultCheck.HitNumbers, Is.EqualTo(expectedHitNumbers));
    }

    [Test]
    public void CheckResult_ShouldReturnAllNumbers_WhenAllNumbersAreAHit()
    {
        var expectedHitNumbers = new List<int> { 1, 2, 3, 4, 5 };
        var expectedHits = 5;

        var result = new Result
        {
            Id = 0,
            LottoId = string.Empty,
            Numbers = new List<int> { 1, 2, 3, 4, 5 },
        };

        var resultCheck = result.CheckResult(new List<int> { 1, 2, 3, 4, 5 });

        Assert.That(resultCheck.Hits, Is.EqualTo(expectedHits));
        Assert.That(resultCheck.HitNumbers, Is.EqualTo(expectedHitNumbers));
    }

    [Test]
    public void CheckResult_ShouldReturnAllNumbers_WhenLineHasMoreNumbers_AndAllNumbersAreHit()
    {
        var expectedHitNumbers = new List<int> { 3, 4, 5, 6, 7 };
        var expectedHits = 5;

        var result = new Result
        {
            Id = 0,
            LottoId = string.Empty,
            Numbers = new List<int> { 3, 4, 5, 6, 7 },
        };

        var resultCheck = result.CheckResult(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

        Assert.That(resultCheck.Hits, Is.EqualTo(expectedHits));
        Assert.That(resultCheck.HitNumbers, Is.EqualTo(expectedHitNumbers));
    }

    [Test]
    public void CheckResult_ShouldReturnAllNumbers_WhenLineHasMoreNumbers_AndNoNumbersAreAHit()
    {
        var expectedHitNumbers = new List<int>();
        var expectedHits = 0;

        var result = new Result
        {
            Id = 0,
            LottoId = string.Empty,
            Numbers = new List<int> { 3, 4, 5, 6, 7 },
        };

        var resultCheck = result.CheckResult(new List<int> { 1, 8, 9, 10, 11 });

        Assert.That(resultCheck.Hits, Is.EqualTo(expectedHits));
        Assert.That(resultCheck.HitNumbers, Is.EqualTo(expectedHitNumbers));
    }
}
