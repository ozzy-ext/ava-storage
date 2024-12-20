using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Tests
{
    public class PictureValidatorBehavior
    {
        [Theory]
        [InlineData(1200, 1200, false)]
        [InlineData(100, 100, true)]
        public async Task ShouldValidateSize(int imgWidth, int imgHeight, bool expectedValidation)
        {
            //Arrange
            var validator = new PictureValidator(512);
            var avaPicture = new AvatarPicture
            (
                new AvatarPictureBin(new byte[] { 1, 2, 3 }),
                new PictureSize(imgWidth, imgHeight)
            );


            //Act
            bool validationResult = validator.IsValid(avaPicture!);

            //Assert
            Assert.Equal(expectedValidation, validationResult);
        }
    }
}
