using AvaStorage.Application.UseCases.GetAvatar;
using AvaStorage.Domain.ValueObjects;
using AvaStorage.Infrastructure.Services;
using Moq;

namespace AvaStorage.Application.Tests
{
    public partial class GetAvatarHandlerBehavior
    {
        
        [Fact]
        public async Task ShouldGetDefaultPictureIfNoRequiredSize()
        {
            //Arrange
            _picRepoMock
                .Setup(r => r.LoadOriginalPersonalPictureAsync("foo"))
                .ReturnsAsync(_testAva64);
            
            var getCmd = new GetAvatarCommand("foo", null, null);

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
            _picToolsMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetPictureWithSize()
        {
            //Arrange
            _picRepoMock
                .Setup(r => r.LoadPersonalPictureWithSizeAsync("foo", 64))
                .ReturnsAsync(_testAva64);

            var getCmd = new GetAvatarCommand("foo", 64, null);

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
            _picToolsMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetSubjectTypePictureIfPersonalNotFound()
        {
            //Arrange
            _picRepoMock
                .Setup(r => r.LoadSubjectTypePictureWithSizeAsync("bar", 64))
                .ReturnsAsync(_testAva64);

            var getCmd = new GetAvatarCommand("foo", 64, "bar");

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
            _picToolsMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetDefaultSubjectTypeIfNoSubjectTypeWithSize()
        {
            //Arrange
            _picRepoMock
                .Setup(r => r.LoadDefaultSubjectTypePictureAsync("bar"))
                .ReturnsAsync(_testAva64);

            var getCmd = new GetAvatarCommand("foo", 64, "bar");

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
            _picToolsMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetDefaultWithSizeIfNoSubjectPic()
        {
            //Arrange
            _picRepoMock
                .Setup(r => r.LoadDefaultPictureWithSizeAsync(64))
                .ReturnsAsync(_testAva64);

            var getCmd = new GetAvatarCommand("foo", 64, "bar");

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
            _picToolsMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetDefaultIfNoDefaultWithSize()
        {
            //Arrange
            _picRepoMock
                .Setup(r => r.LoadDefaultPictureAsync())
                .ReturnsAsync(_testAva64);

            var getCmd = new GetAvatarCommand("foo", 64, "bar");

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
            _picToolsMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNormalizePicture()
        {
            //Arrange
            var originPic = new AvatarPicture(TestPicBin, new PictureSize(128, 64));
            var modifiedPic = new AvatarPicture(new byte[]{3, 2, 1}, new PictureSize(64, 64));

            _picRepoMock
                .Setup(r => r.LoadDefaultPictureAsync())
                .ReturnsAsync(originPic);

            var picToolsMock = new Mock<IPictureTools>();
            picToolsMock
                .Setup(t => t.NormalizeAsync
                    (
                        It.IsAny<AvatarPicture>(), 
                        It.IsAny<int>(), 
                        It.IsAny<CancellationToken>()
                    ))
                .ReturnsAsync(modifiedPic);

            var getCmd = new GetAvatarCommand("foo", 64, "bar");

            var handler = new GetAvatarHandler(DefaultOptions, _picRepoMock.Object, picToolsMock.Object);

            //Act
            var result = await handler.Handle(getCmd, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.AvatarPicture);
            Assert.Equal(modifiedPic.Binary, result.AvatarPicture);
            _picToolsMock.VerifyNoOtherCalls();
        }
    }
}
