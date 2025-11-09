using LanguageExt;
using LanguageExt.Common;

namespace Biblio.Common.Tests
{
    public class FinExtensionsTests
    {
        [Fact]
        public void IsSuccess_ReturnsTrueAndValue_When_FinIsSuccess()
        {
            // Arrange
            Fin<int> fin = 42; // implicit success

            // Act
            var result = Biblio.Common.Extensions.FinExtensions.IsSuccess(fin, out var value, out var error);

            // Assert
            Assert.True(result);
            Assert.Equal(42, value);
            Assert.Null(error);
        }

        [Fact]
        public void IsSuccess_ReturnsFalseAndError_When_FinIsFailure()
        {
            // Arrange
            var err = Error.New("boom");
            Fin<int> fin = err; // implicit failure

            // Act
            var result = Extensions.FinExtensions.IsSuccess(fin, out var value, out var error);

            // Assert
            Assert.False(result);
            Assert.Equal(default(int), value);
            Assert.NotNull(error);
            Assert.Equal("boom", error.Message);
        }

        [Fact]
        public void IsFailure_ReturnsFalse_When_FinIsSuccess()
        {
            // Arrange
            Fin<int> fin = 42; // implicit success

            // Act
            var result = Extensions.FinExtensions.IsFailure(fin, out var error);

            // Assert
            Assert.False(result);
            Assert.Null(error);
        }

        [Fact]
        public void IsFailure_ReturnsTrueAndError_When_FinIsFailure()
        {
            // Arrange
            var err = Error.New("boom");
            Fin<int> fin = err; // implicit failure

            // Act
            var result = Extensions.FinExtensions.IsFailure(fin, out var error);

            // Assert
            Assert.True(result);
            Assert.NotNull(error);
            Assert.Equal("boom", error.Message);
        }
    }
}
