using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Tests
{
    public class PictureValidatorBehavior
    {
        [Theory]
        [InlineData("1200.jpg", false)]
        [InlineData("norm.jpg", true)]
        public void ShouldValidateSize(string filename, bool expectedValidation)
        {
            //Arrange
            var validator = new PictureValidator
            {
                MaxPictureSize = 512
            };

            var pictureBin = File.ReadAllBytes(Path.Combine("files", filename));

            AvatarPicture.TryLoad(pictureBin, out var avaPicture);

            //Act
            bool validationResult = validator.IsValid(avaPicture!);

            //Assert
            Assert.Equal(expectedValidation, validationResult);
        }
    }
}
