using System.ComponentModel.DataAnnotations;
using AvaStorage.Application.Options;
using AvaStorage.Application.UseCases.PutAvatar;
using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;
using AvaStorage.Infrastructure.ImageSharp;
using AvaStorage.Infrastructure.ImageSharp.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace AvaStorage.Application.Tests
{
    public class PutAvatarHandlerBehavior
    {
        [Fact]
        public async Task ShouldSaveOriginalPicture()
        {
            //Arrange
            var repo = new Mock<IPictureRepository>();
            var options = new OptionsWrapper<AvaStorageOptions>(new AvaStorageOptions());
            var handler = new PutAvatarHandler(options, repo.Object, new ImageSharpPictureTools());

            var picBin = await File.ReadAllBytesAsync("files\\norm.jpg");

            var putCmd = new PutAvatarCommand
            (
                "foo",
                picBin
            );

            var expectedPicAddr = new OriginalPersonalPicAddrProvider("foo").ProvideAddress();

            //Act
            await handler.Handle(putCmd, CancellationToken.None);

            //Assert
            repo.Verify
            (
                r => r.SavePictureAsync
                (
                    It.Is<IPictureAddressProvider>(p => p.ProvideAddress() == expectedPicAddr),
                    It.IsAny<AvatarPictureBin>(),
                    CancellationToken.None
                )
            );
            repo.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(GetInvalidParameters))]
        public async Task ShouldFailIfInvalidParameters(string id, byte[] picBin)
        {
            //Arrange
            var repo = new Mock<IPictureRepository>();
            var options = new OptionsWrapper<AvaStorageOptions>(new AvaStorageOptions());
            var handler = new PutAvatarHandler(options, repo.Object, new ImageSharpPictureTools());

            var putCmd = new PutAvatarCommand
            (
                id,
                picBin
            );

            //Act & Assert

            await Assert.ThrowsAsync<ValidationException>
            (
                () => handler.Handle(putCmd, CancellationToken.None)
            );
        }

        public static object[][] GetInvalidParameters()
        {
            var validPicBin = File.ReadAllBytes("files\\norm.jpg");
            var invalidPicBin = new byte[] { 1, 2, 3 };

            return new object[][]
            {
                new object[] { "foo", invalidPicBin},
                new object[] { "", validPicBin}
            };
        }
    }
}