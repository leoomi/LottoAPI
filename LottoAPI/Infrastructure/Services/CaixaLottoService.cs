using System.Text.Json;
using LottoAPI.Domain.Entities;
using LottoAPI.Infrastructure.Models;

namespace LottoAPI.Infrastructure.Services;

public class CaixaLottoService : IExternalLottoService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CaixaLottoService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result> GetResult(string lottoId, int? resultId = null)
    {
        var client = _httpClientFactory.CreateClient();
        var externalResult = await client.GetStringAsync(
            $"https://servicebus2.caixa.gov.br/portaldeloterias/api/{lottoId}/{resultId}"
        );
        Console.WriteLine(externalResult);

        var result = JsonSerializer.Deserialize<CaixaLottoResult>(externalResult);

        return new Result
        {
            Id = result!.Id,
            LottoId = lottoId,
            Numbers = result.Numbers.Select(n => int.Parse(n)).ToList(),
        };
    }
}
