using AvaStorage.Application.Options;
using AvaStorage.Application.UseCases.GetAvatar;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;
using AvaStorage.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace AvaStorage.Application.Tests;

public partial class GetAvatarHandlerBehavior
{
    private static readonly byte[] TestPicBin = new byte[] { 1, 2, 3 };

    private static readonly IOptions<AvaStorageOptions> DefaultOptions =
        new OptionsWrapper<AvaStorageOptions>(new AvaStorageOptions());

    private readonly Mock<IPictureRepository> _picRepoMock = new();
    private readonly Mock<IPictureTools> _picToolsMock = new();
    private readonly GetAvatarHandler _handler;

    private readonly AvatarPicture _absentPicture = null;

    private readonly AvatarPicture _testAva64 = new (TestPicBin, new PictureSize(64, 64));

    public GetAvatarHandlerBehavior()
    {
        _handler = new GetAvatarHandler(DefaultOptions, _picRepoMock.Object, _picToolsMock.Object);
    }

    private void AssertTheSamePicture(GetAvatarResult avatarResult)
    {
        Assert.NotNull(avatarResult);
        Assert.NotNull(avatarResult.AvatarPicture);
        Assert.Equal(TestPicBin, avatarResult.AvatarPicture);
    }
}