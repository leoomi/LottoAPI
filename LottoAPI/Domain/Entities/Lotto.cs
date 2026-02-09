namespace LottoAPI.Domain.Entities;

public class Lotto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public long? LastResultId { get; set; } // TODO: Not sure if I'll use this
}
