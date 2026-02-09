using LottoAPI.Domain.Entities;

namespace LottoAPI.Infrastructure.Repositories;

public interface IResultRepository
{
    Task<Result?> GetResult(string lottoId, int id);

    Task<IList<Result>> GetAll(string lottoId);

    Task<Result> Add(Result result);
    // Task<Lotto> Update(Lotto lotto);
}
