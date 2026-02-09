using System.Text.Json;
using LottoAPI.Application.Requests;
using LottoAPI.Domain.Entities;
using LottoAPI.Infrastructure.Repositories;
using LottoAPI.Infrastructure.Services;

namespace LottoAPI.Application.Features.NumberChecking;

public class CheckLottoLineHandler : ICheckLottoLineUseCase
{
    private readonly IExternalLottoService _externalLottoService;
    private readonly IResultRepository _resultRepository;
    private readonly ILottoRepository _lottoRepository;

    public CheckLottoLineHandler(
        IExternalLottoService caixaLottoService,
        IResultRepository resultRepository,
        ILottoRepository lottoRepository
    )
    {
        _externalLottoService = caixaLottoService;
        _resultRepository = resultRepository;
        _lottoRepository = lottoRepository;
    }

    public async Task<IList<ResultCheck>> CheckLottoNumbers(
        string lottoId,
        CheckLottoLineRequest request
    )
    {
        var lottos = await _lottoRepository.GetAll();

        if (!lottos.Any(l => l.Id == lottoId))
        {
            // TODO ideally have a exception middleware and a custom exception to map to 404
            throw new Exception("lotto id not found");
        }

        // TODO cache result inside get result to avoid calling the service every time
        var lastResult = await _externalLottoService.GetResult(lottoId);
        var to = request.To;
        if (lastResult.Id < to)
        {
            to = lastResult.Id;
        }

        var resultChecks = new List<ResultCheck>();
        foreach (var id in Enumerable.Range(request.From, to - request.From - 1))
        {
            var result = await _resultRepository.GetResult(lottoId, id);
            if (result == null)
            {
                result = await _externalLottoService.GetResult(lottoId, id);
                await _resultRepository.Add(result);
            }

            resultChecks.Add(result.CheckResult(request.Numbers));
        }

        return resultChecks;
    }
}
