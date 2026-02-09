using LottoAPI.Domain.Entities;

namespace LottoAPI.Infrastructure.Repositories;

public interface ILottoRepository
{
    Task<IList<Lotto>> GetAll();

    Task Delete(string id);

    Task<Lotto> Add(Lotto lotto);
    // Task<Lotto> Update(Lotto lotto);
}
