using AvaStorage.Application.UseCases.GetAvatar;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLab.ApiClient.Test;
using MyLab.AvaStorage;
using MyLab.WebErrors;
using Xunit.Abstractions;

namespace AvaStorage.Tests;

public partial class GetApiBehavior
{
    private readonly TestApiFixture<Program, IAvaStorageV1> _fxt;

    public GetApiBehavior(TestApiFixture<Program, IAvaStorageV1> fxt, ITestOutputHelper output)
    {
        fxt.Output = output;
        _fxt = fxt;
        _fxt.ServiceOverrider = srv => srv.Configure<ExceptionProcessingOptions>(o => o.HideError = false);
    }

    private readonly Mock<IRequestHandler<GetAvatarCommand, GetAvatarResult>> _getHandlerMock = new();

    private void VerifyHandlerCall(string id, int? size, string? subjectType)
    {
        _getHandlerMock.Verify(h => h.Handle
            (
                It.Is<GetAvatarCommand>(c =>
                    c.Id == id &&
                    c.Size == size &&
                    c.SubjectType == subjectType
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
        _getHandlerMock.VerifyNoOtherCalls();
    }

    private IAvaStorageV1 CreateClient()
    {
        var proxyAsset = _fxt.StartWithProxy(TestTools.AddGetMocks(TestTools.DefaultRepoMock, _getHandlerMock));
        return proxyAsset.ApiClient;
    }
}