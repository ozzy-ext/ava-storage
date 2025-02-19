using System.ComponentModel.DataAnnotations;
using AutoFixture;
using AvaStorage.Application.Options;
using AvaStorage.Application.Services;
using AvaStorage.Application.UseCases.PutAvatar;
using AvaStorage.Domain;
using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.Tools;
using Microsoft.Extensions.Options;
using Moq;
using Xunit.Abstractions;

namespace AvaStorage.Application.Tests
{
    public class PutAvatarHandlerBehavior
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<IPictureRepository> _repo;
        private readonly Mock<IImageModifier> _imageModifier;
        private readonly Mock<IImageMetadataExtractor> _pngMetadataExtractor;
        private readonly byte[] _mockImgBin;

        public PutAvatarHandlerBehavior(ITestOutputHelper output)
        {
            _output = output;

            var fixture = new Fixture();
            _repo = new Mock<IPictureRepository>();
            _imageModifier = new Mock<IImageModifier>();
            
            _pngMetadataExtractor = new Mock<IImageMetadataExtractor>();
            _pngMetadataExtractor.Setup(e => e.ExtractAsync
                (
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()
                )
            ).Returns<Stream, CancellationToken>((_, _) => Task.FromResult(new ImageMetadata(10, 10, "PNG")));

            _mockImgBin = fixture.Create <byte[]>();
        }

        [Fact]
        public async Task ShouldSaveAsOriginalPicture()
        {
            //Arrange
            var handler = new PutAvatarHandler(_repo.Object, _pngMetadataExtractor.Object, _imageModifier.Object);
            var putCmd = new PutAvatarCommand("foo", _mockImgBin);

            var expectedPicAddr = new OriginalPersonalPicAddrProvider("foo").ProvideAddress();

            //Act
            await handler.Handle(putCmd, CancellationToken.None);

            //Assert
            _repo.Verify
            (
                r => r.SavePictureAsync
                (
                    It.Is<IPictureAddressProvider>(p => p.ProvideAddress() == expectedPicAddr),
                    It.IsAny<IAvatarFile>(),
                    CancellationToken.None
                )
            );
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldConvertToInnerFormatIfOriginIsAnother()
        {
            //Arrange
            var imageModifier = new Mock<IImageModifier>();

            var jpgMetadataExtractor = new Mock<IImageMetadataExtractor>();
            jpgMetadataExtractor.Setup(e => e.ExtractAsync
                (
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()
                )
            ).Returns<Stream, CancellationToken>((_, _) => Task.FromResult(new ImageMetadata(10, 10, "JPG")));

            var handler = new PutAvatarHandler(_repo.Object, jpgMetadataExtractor.Object, imageModifier.Object);
            var putCmd = new PutAvatarCommand("foo", _mockImgBin);

            //Act
            await handler.Handle(putCmd, CancellationToken.None);

            //Assert
            imageModifier.Verify
            (
                r => r.ConvertToInnerFormatAsync
                (
                    It.IsAny<IAvatarFile>(),
                    It.IsAny<CancellationToken>()
                )
            );
            imageModifier.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotConvertToInnerFormatIfOriginIsInner()
        {
            //Arrange
            var imageModifier = new Mock<IImageModifier>();

            var handler = new PutAvatarHandler(_repo.Object, _pngMetadataExtractor.Object, imageModifier.Object);
            var putCmd = new PutAvatarCommand("foo", _mockImgBin);

            //Act
            await handler.Handle(putCmd, CancellationToken.None);

            //Assert
            imageModifier.Verify
            (
                r => r.ConvertToInnerFormatAsync
                (
                    It.IsAny<IAvatarFile>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never
            );
            imageModifier.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldFailIfInvalidAvatarId()
        {
            //Arrange
            var handler = new PutAvatarHandler(_repo.Object, _pngMetadataExtractor.Object, _imageModifier.Object);
            var putCmd = new PutAvatarCommand("", _mockImgBin);

            //Act & Assert

            var  e = await Assert.ThrowsAsync<ValidationException>
            (
                () => handler.Handle(putCmd, CancellationToken.None)
            );

            _output.WriteLine(e.ToString());
        }

        [Fact]
        public async Task ShouldFailIfInvalidImageFormat()
        {
            //Arrange
            var metadataExtractor = new Mock<IImageMetadataExtractor>();

            metadataExtractor.Setup(e => e.ExtractAsync
                (
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()
                )
            ).Throws<Exception>(() => new InvalidOperationException());
            var handler = new PutAvatarHandler(_repo.Object, metadataExtractor.Object, _imageModifier.Object);

            var putCmd = new PutAvatarCommand("foo", _mockImgBin);

            //Act & Assert
            var e = await Assert.ThrowsAsync<ValidationException>
            (
                () => handler.Handle(putCmd, CancellationToken.None)
            );

            _output.WriteLine(e.ToString());
        }

        [Fact]
        public async Task ShouldSavePredefinedSizeCopies()
        {
            //Arrange
            var avaOptions = new AvaStorageOptions
            {
                PredefinedSizes = [10, 20]
            };

            var handler = new PutAvatarHandler
                (
                    _repo.Object, 
                    _pngMetadataExtractor.Object, 
                    _imageModifier.Object,
                    new OptionsWrapper<AvaStorageOptions>(avaOptions)
                );
            var putCmd = new PutAvatarCommand("foo", _mockImgBin);

            var expectedOriginalPicAddr = new OriginalPersonalPicAddrProvider("foo").ProvideAddress();
            var expected10SizePicAddr = new PersonalWithSizePicAddrProvider("foo", 10).ProvideAddress();
            var expected20SizePicAddr = new PersonalWithSizePicAddrProvider("foo", 20).ProvideAddress();

            //Act
            await handler.Handle(putCmd, CancellationToken.None);

            //Assert
            _repo.Verify
            (
                r => r.SavePictureAsync
                (
                    It.Is<IPictureAddressProvider>
                    (p => 
                        p.ProvideAddress() == expectedOriginalPicAddr
                    ),
                    It.IsAny<IAvatarFile>(),
                    CancellationToken.None
                )
            );
            _repo.Verify
            (
                r => r.SavePictureAsync
                (
                    It.Is<IPictureAddressProvider>
                    (p => 
                        p.ProvideAddress() == expected10SizePicAddr
                    ),
                    It.IsAny<IAvatarFile>(),
                    CancellationToken.None
                )
            );
            _repo.Verify
            (
                r => r.SavePictureAsync
                (
                    It.Is<IPictureAddressProvider>
                    (p => 
                        p.ProvideAddress() == expected20SizePicAddr
                    ),
                    It.IsAny<IAvatarFile>(),
                    CancellationToken.None
                )
            );
            _repo.VerifyNoOtherCalls();
        }
    }
}