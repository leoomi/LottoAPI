using System.Text.Json.Serialization;

namespace LottoAPI.Infrastructure.Models;

public record CaixaLottoResult(
    [property: JsonPropertyName("numero")] int Id,
    [property: JsonPropertyName("listaDezenas")] IList<string> Numbers
);
