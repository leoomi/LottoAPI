using LottoAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LottoAPI.Infrastructure.Repositories;

public class LottoRepository : ILottoRepository
{
    private readonly LottoDbContext _context;

    public LottoRepository(LottoDbContext lottoDbContext)
    {
        _context = lottoDbContext;
    }

    public async Task<IList<Lotto>> GetAll()
    {
        return await _context.Lottos.ToListAsync();
    }

    public async Task Delete(string id)
    {
        var lotto = await _context.Lottos.FirstAsync(l => l.Id == id);
        _context.Lottos.Remove(lotto);
        await _context.SaveChangesAsync();
    }

    public async Task<Lotto> Add(Lotto lotto)
    {
        _context.Lottos.Add(lotto);
        await _context.SaveChangesAsync();

        return lotto;
    }

    // public async Task<Lotto> Update(Lotto lotto);
}
