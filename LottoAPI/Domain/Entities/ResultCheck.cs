namespace LottoAPI.Domain.Entities;

public class ResultCheck
{
    public required int Id { get; set; }
    public required IList<int> ResultNumbers { get; set; }
    public required IList<int> Line { get; set; }
    public required IList<int> HitNumbers { get; set; }
    public required int Hits { get; set; }
}
