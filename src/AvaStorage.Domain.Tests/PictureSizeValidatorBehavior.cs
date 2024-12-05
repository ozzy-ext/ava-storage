using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaStorage.Domain.Tools;

namespace AvaStorage.Domain.Tests
{
    public class PictureSizeValidatorBehavior
    {
        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(8, false)]
        [InlineData(1000, false)]
        [InlineData(100, true)]
        public void ShouldValidateSize(int size, bool expectedResult)
        {
            //Arrange
            var validator = new PictureSizeValidator(512);

            //Act
            var result = validator.IsValid(size);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
