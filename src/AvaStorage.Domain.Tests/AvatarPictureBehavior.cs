using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Tests
{
    public class AvatarPictureBehavior
    {
        [Theory]
        [MemberData(nameof(GetAllNormFilenames))]
        public void ShouldLoadPicture(string filename)
        {
            //Arrange
            var pictureBin = File.ReadAllBytes(filename);

            //Act
            var success = AvatarPicture.TryLoad(pictureBin, out var picture);
            picture?.Dispose();

            //Assert
            Assert.True(success);
            Assert.NotNull(picture);
            Assert.NotNull(picture.Image);
            Assert.Equal(436,picture.Image.Width);
            Assert.Equal(395,picture.Image.Height);
        }

        [Fact]
        public void ShouldNotLoadIfWrongFormat()
        {
            //Arrange
            var pictureBin = File.ReadAllBytes("files\\wrong-format.txt");

            //Act
            var success = AvatarPicture.TryLoad(pictureBin, out var picture);
            
            //Assert
            Assert.False(success);
            Assert.Null(picture);

        }

        public static object[][] GetAllNormFilenames()
        {
            return Directory
                .EnumerateFiles("files", "norm.*")
                .Select(fn => new object[]{fn})
                .ToArray();
        }
    }
}