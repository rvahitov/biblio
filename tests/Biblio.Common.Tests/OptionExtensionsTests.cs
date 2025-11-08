using LanguageExt;

namespace Biblio.Common.Tests
{
    public class OptionExtensionsTests
    {
        [Fact]
        public void IsJust_ReturnsTrueAndValue_When_OptionIsSome()
        {
            // Arrange
            Option<int> opt = 7; // Some(7)

            // Act
            var result = Extensions.OptionExtensions.IsJust(opt, out var value);

            // Assert
            Assert.True(result);
            Assert.Equal(7, value);
        }

        [Fact]
        public void IsJust_ReturnsFalseAndDefault_When_OptionIsNone()
        {
            // Arrange
            Option<int> opt = Option<int>.None;

            // Act
            var result = Extensions.OptionExtensions.IsJust(opt, out var value);

            // Assert
            Assert.False(result);
            Assert.Equal(default, value);
        }
    }
}
