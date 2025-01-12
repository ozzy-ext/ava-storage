using AvaStorage.Application.Services;
using AvaStorage.Application.UseCases.GetAvatar;
using AvaStorage.Domain;
using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.ValueObjects;
using Moq;

namespace AvaStorage.Application.Tests
{
    public partial class GetAvatarHandlerBehavior
    {
        
        [Fact]
        public async Task ShouldGetOriginalPictureIfSizeIsNotRequired()
        {
            //Arrange
            var expectedPicAddr = new OriginalPersonalPicAddrProvider("foo").ProvideAddress();
            _picRepoMock
                .Setup(r => r.LoadPictureAsync(
                    It.Is<IPictureAddressProvider>(p => p.ProvideAddress() == expectedPicAddr), 
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(_testAva64);
            
            var getCmd = new GetAvatarCommand("foo", null, null);

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
        }

        [Fact]
        public async Task ShouldGetOriginalPictureIfSizedNotFound()
        {
            //Arrange
            var expectedPicAddr = new OriginalPersonalPicAddrProvider("foo").ProvideAddress();
            _picRepoMock
                .Setup(r => r.LoadPictureAsync(
                    It.Is<IPictureAddressProvider>(p => p.ProvideAddress() == expectedPicAddr),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(_testAva64);

            var getCmd = new GetAvatarCommand("foo", 64, null);

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
        }

        [Fact]
        public async Task ShouldGetPictureWithSize()
        {
            //Arrange
            var expectedPicAddr = new PersonalWithSizePicAddrProvider("foo", 64).ProvideAddress();
            _picRepoMock
                .Setup(r => r.LoadPictureAsync(
                    It.Is<IPictureAddressProvider>(p => p.ProvideAddress() == expectedPicAddr),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(_testAva64);

            var getCmd = new GetAvatarCommand("foo", 64, null);

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
        }

        [Fact]
        public async Task ShouldGetSubjectTypePictureIfPersonalNotFound()
        {
            //Arrange
            var expectedPicAddr = new DefaultSubjectTypeWithSizeAddPicProvider("bar", 64).ProvideAddress();
            _picRepoMock
                .Setup(r => r.LoadPictureAsync(
                    It.Is<IPictureAddressProvider>(p => p.ProvideAddress() == expectedPicAddr),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(_testAva64);

            var getCmd = new GetAvatarCommand("foo", 64, "bar");

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
        }

        [Fact]
        public async Task ShouldGetDefaultSubjectTypeIfNoSubjectTypeWithSize()
        {
            //Arrange
            var expectedPicAddr = new DefaultSubjectTypePicAddrProvider("bar").ProvideAddress();
            _picRepoMock
                .Setup(r => r.LoadPictureAsync(
                    It.Is<IPictureAddressProvider>(p => p.ProvideAddress() == expectedPicAddr),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(_testAva64);

            var getCmd = new GetAvatarCommand("foo", 64, "bar");

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
        }

        [Fact]
        public async Task ShouldGetDefaultWithSizeIfNoSubjectPic()
        {
            //Arrange
            var expectedPicAddr = new DefaultPicWithSizeAddrProvider(64).ProvideAddress();
            _picRepoMock
                .Setup(r => r.LoadPictureAsync(
                    It.Is<IPictureAddressProvider>(p => p.ProvideAddress() == expectedPicAddr),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(_testAva64);

            var getCmd = new GetAvatarCommand("foo", 64, "bar");

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
        }

        [Fact]
        public async Task ShouldGetDefaultIfNoDefaultWithSize()
        {
            //Arrange
            var expectedPicAddr = new DefaultPicAddrProvider().ProvideAddress();
            _picRepoMock
                .Setup(r => r.LoadPictureAsync(
                    It.Is<IPictureAddressProvider>(p => p.ProvideAddress() == expectedPicAddr),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(_testAva64);

            var getCmd = new GetAvatarCommand("foo", 64, "bar");

            //Act
            var result = await _handler.Handle(getCmd, CancellationToken.None);

            //Assert
            AssertTheSamePicture(result);
        }

        [Fact]
        public async Task ShouldNormalizePicture()
        {
            //Arrange
            var avaFileMock = new Mock<IAvatarFile>();

            var modifiedPic = avaFileMock.Object;

            var expectedPicAddr = new DefaultPicAddrProvider().ProvideAddress();

            _picRepoMock
                .Setup(r => r.LoadPictureAsync(
                    It.Is<IPictureAddressProvider>(p => p.ProvideAddress() == expectedPicAddr),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(_testAva64);

            var imgModifierMock = new Mock<IImageModifier>();

            imgModifierMock
                .Setup(t => t.FitIntoSizeAsync
                    (
                        It.IsAny<IAvatarFile>(), 
                        It.IsAny<int>(), 
                        It.IsAny<CancellationToken>()
                    ))
                .ReturnsAsync(modifiedPic);

            var getCmd = new GetAvatarCommand("foo", 64, "bar");

            var handler = new GetAvatarHandler(DefaultOptions, _picRepoMock.Object, imgModifierMock.Object);

            //Act
            var result = await handler.Handle(getCmd, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.AvatarFile);
            Assert.Equal(modifiedPic, result.AvatarFile);
        }
    }
}
