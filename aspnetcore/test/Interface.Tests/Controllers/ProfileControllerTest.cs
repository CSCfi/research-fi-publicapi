using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CSC.PublicApi.Interface.Controllers;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ResearchFi.ProfilePublicApi;
using Serilog;
using Xunit;

namespace CSC.PublicApi.Interface.Tests.Controllers;

public class ProfileControllerTest
{
    [Fact]
    public async Task PostProfile_WhenProfileBackendReturnsBadRequest_ReturnsBadRequest()
    {
        var profileBackendClient = new Mock<IProfileBackendClient>();
        profileBackendClient
            .Setup(client => client.SearchProfilesAsync(
                It.IsAny<ProfileDataRequest>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((
                Response: (ProfileDataResponse?)null,
                ErrorStatusCode: (HttpStatusCode?)HttpStatusCode.BadRequest,
                ErrorContent: "A request body is required.",
                ErrorContentType: "text/plain"));

        var controller = new ProfileController(
            NullLogger<ProfileController>.Instance,
            new Mock<IDiagnosticContext>().Object,
            profileBackendClient.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = await controller.PostProfile(null!);

        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, contentResult.StatusCode);
        Assert.Equal("A request body is required.", contentResult.Content);
        Assert.Equal("text/plain", contentResult.ContentType);
    }
}
