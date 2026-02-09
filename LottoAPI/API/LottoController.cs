using LottoAPI.Application.Features.NumberChecking;
using LottoAPI.Application.Requests;
using LottoAPI.Domain.Entities;
using LottoAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LottoAPI.API;

[ApiController]
[Route("[controller]")]
public class LottosController : ControllerBase
{
    private readonly ILottoRepository _lottoRepository;
    private readonly IResultRepository _resultRepository;
    private readonly ICheckLottoLineUseCase _checkLottoLineUseCase;

    public LottosController(
        ILottoRepository lottoRepository,
        IResultRepository resultRepository,
        ICheckLottoLineUseCase checkLottoLineUseCase
    )
    {
        _lottoRepository = lottoRepository;
        _resultRepository = resultRepository;
        _checkLottoLineUseCase = checkLottoLineUseCase;
    }

    [HttpGet(Name = "GetLottos")]
    public async Task<IList<Lotto>> Get()
    {
        return await _lottoRepository.GetAll();
    }

    [HttpPost(Name = "AddLotto")]
    public async Task<Lotto> Add([FromBody] Lotto lotto)
    {
        return await _lottoRepository.Add(lotto);
    }

    [HttpDelete("{id}", Name = "DeleteLotto")]
    public async Task Delete(string id)
    {
        await _lottoRepository.Delete(id);
    }

    [HttpPost("{id}/lines", Name = "CheckLottoLine")]
    public async Task<IList<ResultCheck>> CheckLottoLine(
        string id,
        [FromBody] CheckLottoLineRequest request
    )
    {
        return await _checkLottoLineUseCase.CheckLottoNumbers(id, request);
    }

    [HttpGet("{id}/result", Name = "GetAllResults")]
    public async Task<IList<Result>> GetAllResults(string id)
    {
        return await _resultRepository.GetAll(id);
    }
}
