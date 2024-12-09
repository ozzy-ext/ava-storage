using AvaStorage.Application;
using AvaStorage.Application.Tools;

namespace AvaStorage.Infrastructure.Tests
{
    public class PictureResizeCalculatorBehavior
    {
        [Theory]
        [MemberData(nameof(GetSizeCases))]
        public void ShouldCalculateNewSize(ImageSize origin, ImageSize expected)
        {
            //Arrange
            

            //Act
            var result = PictureResizeCalculator.Calculate(origin, 500);

            //Assert
            Assert.Equal(expected, result);
        }

        public static object[][] GetSizeCases()
        {
            var seeds = new[]
            {
                new { Value1 = new ImageSize(200, 100), Value2 = new ImageSize(1000, 500) },
                new { Value1 = new ImageSize(200, 200), Value2 = new ImageSize(500, 500) },
                new { Value1 = new ImageSize(500, 200), Value2 = new ImageSize(1250, 500) },
                new { Value1 = new ImageSize(600, 200), Value2 = new ImageSize(1500, 500) },
                new { Value1 = new ImageSize(2000, 1000), Value2 = new ImageSize(1000, 500) },
            };

            return seeds.SelectMany(s =>
            {
                return new object[][]
                {
                    new object[] { s.Value1, s.Value2 },
                    new object[]
                    {
                        new ImageSize(s.Value1.Height, s.Value1.Width),
                        new ImageSize(s.Value2.Height, s.Value2.Width)
                    }
                };
            }).ToArray();
        }
    }
}