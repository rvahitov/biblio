using System.Reflection;
using Biblio.Citations.Endpoints.Citations;
using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations;

public class AddCitationSummaryTests
{
    [Fact]
    public void Should_ContainExpectedSummaryAndDescription()
    {
        // Arrange
        var summary = new AddCitationSummary();

        // Act & Assert
        summary.Summary.Should().Be("Adds a new citation to the system.");
        summary.Description.Should().Contain("created citation's identifier");
        summary.Description.Should().Contain("request is validated");
    }

    [Fact]
    public void ResponseExample_ShouldContainPersistentCitationId()
    {
        // Use reflection to access the private static ResponseExample field
        var field = typeof(AddCitationSummary).GetField("ResponseExample", BindingFlags.NonPublic | BindingFlags.Static);
        field.Should().NotBeNull();

        var example = field!.GetValue(null) as AddCitationResponse;
        example.Should().NotBeNull();
        example!.CitationId.Should().Be("123e4567-e89b-12d3-a456-426614174000");
    }
}
