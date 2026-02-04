using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.ResearchDataset;
using FluentAssertions;
using Xunit;
using Version = CSC.PublicApi.Service.Models.ResearchDataset.Version;

namespace Repository.Tests;

public class RepositoryHelpersTest
{ 
    [Fact]
    public void GetURNLink_ReturnsCorrectUrl()
    {
        // Arrange
        string pid = "abcd1234-5678-90ef-ghij12345678";
        
        // Act
        string urnLink = RepositoryHelpers.GetURNLink(pid);
        
        // Assert
        urnLink.Should().NotBeNullOrEmpty();
        urnLink.Should().Be("https://urn.fi/abcd1234-5678-90ef-ghij12345678");
    }
}