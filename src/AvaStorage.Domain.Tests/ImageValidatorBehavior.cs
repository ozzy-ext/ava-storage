using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Tests
{
    public class ImageValidatorBehavior
    {
        [Theory]
        [InlineData(1200, 1200, false)]
        [InlineData(100, 100, true)]
        public async Task ShouldValidateSize(int imgWidth, int imgHeight, bool expectedValidation)
        {
            //Arrange
            var validator = new ImageValidator(512);
            var avaPictureMetadata = new ImageMetadata(imgWidth, imgWidth, "foo");


            //Act
            bool validationResult = validator.IsValid(avaPictureMetadata!);

            //Assert
            Assert.Equal(expectedValidation, validationResult);
        }
    }
}
