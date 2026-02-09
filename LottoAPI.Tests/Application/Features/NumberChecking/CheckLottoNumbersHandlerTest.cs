using LottoAPI.Application.Features.NumberChecking;
using LottoAPI.Application.Requests;
using LottoAPI.Domain.Entities;
using LottoAPI.Infrastructure.Repositories;
using LottoAPI.Infrastructure.Services;
using Moq;

namespace LottoAPI.Test.Application.Feautures.NumberChecking;

[TestFixture]
public class CheckLottoNumbersHandlerTest
{
    private Mock<IExternalLottoService> _externalLottoServiceMock = new();
    private Mock<IResultRepository> _resultRepositoryMock = new();
    private Mock<ILottoRepository> _lottoRepositoryMock = new();
    private CheckLottoLineHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _externalLottoServiceMock = new();
        _resultRepositoryMock = new();
        _lottoRepositoryMock = new();

        _handler = new(
            _externalLottoServiceMock.Object,
            _resultRepositoryMock.Object,
            _lottoRepositoryMock.Object
        );
    }

    [Test]
    public async Task CheckLottoNumbers_ShouldThrowException_WhenLottoIsNotFound()
    {
        List<Lotto> lottos =
        [
            new() { Id = "lotto1", Name = "Lotto 1" },
            new() { Id = "lotto2", Name = "Lotto 2" },
        ];

        _lottoRepositoryMock.Setup(_ => _.GetAll()).ReturnsAsync(lottos);

        Assert.ThrowsAsync<Exception>(async () =>
            await _handler.CheckLottoNumbers(
                "lotto123",
                new CheckLottoLineRequest(0, 0, new List<int>())
            )
        );
    }

    [Test]
    public async Task CheckLottoNumbers_ShouldReturnCheckResultsAndNotCallExternalService_WhenResultAlreadyExists()
    {
        const int id = 0;
        List<int> numbers = [1, 2, 3, 4, 5];
        List<Lotto> lottos = [new() { Id = "lotto1", Name = "Lotto 1" }];

        _externalLottoServiceMock
            .Setup(m => m.GetResult(It.IsAny<string>(), null))
            .ReturnsAsync(
                new Result
                {
                    Id = 1,
                    LottoId = "lotto1",
                    Numbers = [],
                }
            );
        _lottoRepositoryMock.Setup(m => m.GetAll()).ReturnsAsync(lottos);
        _resultRepositoryMock
            .Setup(m => m.GetResult(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(
                new Result
                {
                    Id = 0,
                    LottoId = "lotto1",
                    Numbers = [1, 2, 3, 4, 5],
                }
            );

        var resultChecks = await _handler.CheckLottoNumbers(
            "lotto1",
            new CheckLottoLineRequest(id, id, numbers)
        );

        Assert.That(resultChecks.Count, Is.EqualTo(1));
        Assert.That(resultChecks[0].Hits, Is.EqualTo(5));
        _externalLottoServiceMock.Verify(m => m.GetResult(It.IsAny<string>(), 1), Times.Never);
    }

    [Test]
    public async Task CheckLottoNumbers_ShouldReturnCheckResultsAndSaveAndCallExternalService_WhenResultDoesNotExist()
    {
        const int id = 0;
        List<int> numbers = [1, 2, 3, 4, 5];
        List<Lotto> lottos = [new() { Id = "lotto1", Name = "Lotto 1" }];

        _externalLottoServiceMock
            .Setup(m => m.GetResult(It.IsAny<string>(), null))
            .ReturnsAsync(
                new Result
                {
                    Id = 1,
                    LottoId = "lotto1",
                    Numbers = [],
                }
            );
        _externalLottoServiceMock
            .Setup(m => m.GetResult(It.IsAny<string>(), 0))
            .ReturnsAsync(
                new Result
                {
                    Id = 0,
                    LottoId = "lotto1",
                    Numbers = [1, 2, 3, 4, 5],
                }
            );
        _lottoRepositoryMock.Setup(m => m.GetAll()).ReturnsAsync(lottos);
        _resultRepositoryMock
            .Setup(m => m.GetResult(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync((Result)null!);

        var resultChecks = await _handler.CheckLottoNumbers(
            "lotto1",
            new CheckLottoLineRequest(id, id, numbers)
        );

        Assert.That(resultChecks.Count, Is.EqualTo(1));
        Assert.That(resultChecks[0].Hits, Is.EqualTo(5));
        _resultRepositoryMock.Verify(m => m.Add(It.Is<Result>(r => r.Id == 0)), Times.Once);
        _externalLottoServiceMock.Verify(m => m.GetResult(It.IsAny<string>(), 1), Times.Never);
    }

    [Test]
    public async Task CheckLottoNumbers_ShouldReturnMultipleResultsChecksAndCheckLastResult_WhenFromAndToAreDifferentAndLastResultIsLessThanTo()
    {
        const int from = 0;
        const int to = 2;
        List<int> numbers = [1, 2, 3, 4, 5];
        List<Lotto> lottos = [new() { Id = "lotto1", Name = "Lotto 1" }];

        _externalLottoServiceMock
            .Setup(m => m.GetResult(It.IsAny<string>(), null))
            .ReturnsAsync(
                new Result
                {
                    Id = 1,
                    LottoId = "lotto1",
                    Numbers = [],
                }
            );
        _lottoRepositoryMock.Setup(m => m.GetAll()).ReturnsAsync(lottos);
        _resultRepositoryMock
            .Setup(m => m.GetResult(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(
                (string lottoId, int id) =>
                    new Result
                    {
                        Id = id,
                        LottoId = lottoId,
                        Numbers = [1, 2, 3, 4, 5],
                    }
            );

        var resultChecks = await _handler.CheckLottoNumbers(
            "lotto1",
            new CheckLottoLineRequest(from, to, numbers)
        );

        Assert.That(resultChecks.Count, Is.EqualTo(2));
        Assert.That(resultChecks[0].Hits, Is.EqualTo(5));
        Assert.That(resultChecks[1].Hits, Is.EqualTo(5));
        _resultRepositoryMock.Verify(m => m.Add(It.IsAny<Result>()), Times.Never);
        _externalLottoServiceMock.Verify(
            m => m.GetResult(It.IsAny<string>(), It.IsAny<int>()),
            Times.Never
        );
    }
}
