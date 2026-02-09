using LottoAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LottoAPI.Infrastructure.Repositories;

public class ResultRepository : IResultRepository
{
    private readonly LottoDbContext _context;

    public ResultRepository(LottoDbContext lottoDbContext)
    {
        _context = lottoDbContext;
    }

    public async Task<IList<Result>> GetAll(string lottoId)
    {
        return await _context.Results.Where(r => r.LottoId == lottoId).ToListAsync();
    }

    public async Task<Result?> GetResult(string lottoId, int id)
    {
        return await _context.Results.FirstOrDefaultAsync(r => r.LottoId == lottoId && r.Id == id);
    }

    // Task<bool> Delete();

    public async Task<Result> Add(Result result)
    {
        _context.Results.Add(result);

        await _context!.SaveChangesAsync();
        return result;
    }

    // Task<Lotto> Update(Lotto lotto);
}
