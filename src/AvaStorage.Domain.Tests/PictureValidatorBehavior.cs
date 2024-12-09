using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;
using AvaStorage.Infrastructure.ImageSharp;

namespace AvaStorage.Domain.Tests
{
    public class PictureValidatorBehavior
    {
        [Theory]
        [InlineData("1200.jpg", false)]
        [InlineData("norm.jpg", true)]
        public async Task ShouldValidateSize(string filename, bool expectedValidation)
        {
            //Arrange
            var validator = new PictureValidator(512);
            var avaPicture = await ImageSharpPictureTools.LoadFromFileAsync(Path.Combine("files",filename), CancellationToken.None);

            //Act
            bool validationResult = validator.IsValid(avaPicture!);

            //Assert
            Assert.Equal(expectedValidation, validationResult);
        }
    }
}
