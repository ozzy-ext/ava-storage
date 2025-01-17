using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Tests
{
    public class ImageValidatorBehavior
    {
        [Theory]
        [InlineData(1200, 1200, false)]
        [InlineData(100, 100, true)]
        public void ShouldValidateWithAndHeight(int imgWidth, int imgHeight, bool expectedValidation)
        {
            //Arrange
            var validator = new ImageValidator(512);
            var avaPictureMetadata = new ImageMetadata(imgWidth, imgWidth, "foo");


            //Act
            bool validationResult = validator.IsValid(avaPictureMetadata!);

            //Assert
            Assert.Equal(expectedValidation, validationResult);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(8, false)]
        [InlineData(1000, false)]
        [InlineData(100, true)]
        public void ShouldValidateSize(int size, bool expectedResult)
        {
            //Arrange
            var validator = new ImageValidator(512);

            //Act
            var result = validator.IsValidSize(size);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
