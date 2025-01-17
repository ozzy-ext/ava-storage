using System.ComponentModel.DataAnnotations;
using AvaStorage.Application.UseCases.PutAvatar;
using AvaStorage.Domain;
using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.Repositories;
using AvaStorage.Infrastructure.ImageSharp.Services;
using Moq;
using Xunit.Abstractions;

namespace AvaStorage.Application.Tests
{
    public class PutAvatarHandlerBehavior
    {
        private readonly ITestOutputHelper _output;

        public PutAvatarHandlerBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldSaveOriginalPicture()
        {
            //Arrange
            var repo = new Mock<IPictureRepository>();
            var handler = new PutAvatarHandler(repo.Object, new ImageSharpImageMetadataExtractor());

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
                    It.IsAny<IAvatarFile>(),
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
            var handler = new PutAvatarHandler(repo.Object, new ImageSharpImageMetadataExtractor());

            var putCmd = new PutAvatarCommand
            (
                id,
                picBin
            );

            //Act & Assert

            var  e = await Assert.ThrowsAsync<ValidationException>
            (
                () => handler.Handle(putCmd, CancellationToken.None)
            );

            _output.WriteLine(e.ToString());
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