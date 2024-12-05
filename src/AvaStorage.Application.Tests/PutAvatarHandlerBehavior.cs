using System.ComponentModel.DataAnnotations;
using AvaStorage.Application.Options;
using AvaStorage.Application.UseCases.PutAvatar;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;
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
            var options = new AvaStorageOptions();

            var handler = new PutAvatarHandler(options, repo.Object);

            var picBin = await File.ReadAllBytesAsync("files\\norm.jpg");

            var putCmd = new PutAvatarCommand
            (
                "foo",
                "bar",
                picBin
            );

            //Act
            await handler.Handle(putCmd, CancellationToken.None);

            //Assert
            repo.Verify
            (
                r => r.SavePictureAsync
                (
                    It.Is<AvatarId>(v => v.Value == "foo"),
                    It.Is<SubjectType>(v => v.Value == "bar"),
                    It.Is<AvatarPicture>(v => v.Image.Width == 436 && v.Image.Height == 395)
                )
            );
            repo.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(GetInvalidParameters))]
        public async Task ShouldFailIfInvalidParameters(string id, string subjectType, byte[] picBin)
        {
            //Arrange
            var repo = new Mock<IPictureRepository>();
            var options = new AvaStorageOptions();

            var handler = new PutAvatarHandler(options, repo.Object);

            var putCmd = new PutAvatarCommand
            (
                id,
                subjectType,
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
                new object[] { "foo", "bar", invalidPicBin},
                new object[] { "", "bar", validPicBin},
                new object[] { "foo", "", validPicBin},
            };
        }
    }
}