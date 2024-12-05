using AvaService.Infrastructure.Tools;

namespace AvaService.Infrastructure.Tests
{
    public class PictureCropCalculatorBehavior
    {
        [Theory]
        [InlineData(200, 100, 50, 0, 100, 100)]
        [InlineData(200, 200, 0, 0, 200, 200)]
        [InlineData(100, 200, 0, 50, 100, 100)]
        public void ShouldCalculateCropRectangle(int originWidth, int originHeight, int expectedX, int expectedY, int expectedWidth, int expectedHeight)
        {
            //Arrange


            //Act
            var result = PictureCropCalculator.CalculateSquare(new ImageSize(originWidth, originHeight));

            //Assert
            Assert.Equal(expectedX, result.X);
            Assert.Equal(expectedY, result.Y);
            Assert.Equal(expectedWidth, result.Width);
            Assert.Equal(expectedHeight, result.Height);
        }
    }
}
