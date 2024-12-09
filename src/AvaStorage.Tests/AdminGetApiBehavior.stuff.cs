using AvaStorage.Application.UseCases.GetAvatar;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLab.ApiClient.Test;
using MyLab.WebErrors;
using Xunit.Abstractions;

namespace AvaStorage.Tests;

public partial class AdminGetApiBehavior
{
    private readonly TestApiFixture<Program, IAdminContractV1> _fxt;

    public AdminGetApiBehavior(TestApiFixture<Program, IAdminContractV1> fxt, ITestOutputHelper output)
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

    private IAdminContractV1 CreateClient()
    {
        var proxyAsset = _fxt.StartWithProxy(TestTools.AddGetMocks(TestTools.DefaultRepoMock, _getHandlerMock),
            TestTools.SetAdminPort);
        return proxyAsset.ApiClient;
    }
}