using LottoAPI.Application.Requests;
using LottoAPI.Domain.Entities;

namespace LottoAPI.Application.Features.NumberChecking;

public interface ICheckLottoLineUseCase
{
    Task<IList<ResultCheck>> CheckLottoNumbers(string lottoId, CheckLottoLineRequest request);
}
