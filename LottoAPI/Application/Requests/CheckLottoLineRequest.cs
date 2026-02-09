namespace LottoAPI.Application.Requests;

public record CheckLottoLineRequest(int From, int To, IList<int> Numbers);
