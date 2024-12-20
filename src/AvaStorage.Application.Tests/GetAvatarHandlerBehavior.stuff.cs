using AutoFixture;
using AvaStorage.Application.Options;
using AvaStorage.Application.Services;
using AvaStorage.Application.UseCases.GetAvatar;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;
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

    private readonly AvatarPictureBin _testAva64 = new (TestPicBin);


    private readonly Fixture _fixture = new Fixture();
    public GetAvatarHandlerBehavior()
    {
        _handler = new GetAvatarHandler(DefaultOptions, _picRepoMock.Object, _picToolsMock.Object);
        _picToolsMock
            .Setup(t => t.DeserializeAsync
            (
                It.IsAny<AvatarPictureBin>(),
                It.IsAny<CancellationToken>()
            ))
            .Returns<AvatarPictureBin, CancellationToken>((bin, _) =>
                Task.FromResult((AvatarPicture?)new AvatarPicture(bin, new PictureSize(64, 64))));
    }

    private void AssertTheSamePicture(GetAvatarResult avatarResult)
    {
        Assert.NotNull(avatarResult);
        Assert.NotNull(avatarResult.AvatarPicture);
        Assert.Equal(TestPicBin, avatarResult.AvatarPicture);
    }
}