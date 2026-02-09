using LottoAPI.Domain.Entities;

namespace LottoAPI.Infrastructure.Services;

public interface IExternalLottoService
{
    Task<Result> GetResult(string lottoId, int? resultId = null);
}
