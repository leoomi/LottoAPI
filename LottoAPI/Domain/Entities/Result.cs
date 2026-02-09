using System.Linq;

namespace LottoAPI.Domain.Entities;

public class Result
{
    public required int Id { get; set; }
    public required string LottoId { get; set; }
    public Lotto? Lotto { get; set; }
    public IList<int> Numbers { get; set; } = new List<int>();

    public ResultCheck CheckResult(IList<int> line)
    {
        var hits = Numbers.Where(n => line.Any(l => l == n)).ToList();

        return new ResultCheck
        {
            Id = Id,
            ResultNumbers = Numbers,
            Line = line,
            HitNumbers = hits,
            Hits = hits.Count(),
        };
    }
}
