using AutoFixture;
using AvaStorage.Application.Options;
using AvaStorage.Application.Services;
using AvaStorage.Application.UseCases.GetAvatar;
using AvaStorage.Domain;
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
    private readonly Mock<IImageModifier> _imgModifierMock = new();
    private readonly GetAvatarHandler _handler;

    private readonly AvatarPicture _absentPicture = null;

    private readonly IAvatarFile _testAva64 = new MemoryAvatarFile(TestPicBin);


    private readonly Fixture _fixture = new Fixture();
    public GetAvatarHandlerBehavior()
    {
        _handler = new GetAvatarHandler(DefaultOptions, _picRepoMock.Object, _imgModifierMock.Object);
        _imgModifierMock
            .Setup(m => m.FitIntoSizeAsync
                (
                    It.IsAny<IAvatarFile>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()
                )
            ).Returns<IAvatarFile, int, CancellationToken>((file, _,_) =>
            Task.FromResult(file));
    }

    private void AssertTheSamePicture(GetAvatarResult avatarResult)
    {
        Assert.NotNull(avatarResult);
        Assert.NotNull(avatarResult.AvatarFile);

        using var readStream = avatarResult.AvatarFile.OpenRead();
        using var mem = new MemoryStream();

        readStream.CopyTo(mem);

        Assert.Equal(TestPicBin, mem.ToArray());
    }
}