using System.Net;
using System.Security.Claims;
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
    [Theory]
    [InlineData(null)]
    [InlineData("synthetic-client")]
    public async Task PostProfile_ForwardsRequestIdentityAndRequestAborted(string? clientId)
    {
        var request = new ProfileDataRequest { ControllerROR = "https://ror.test/synthetic-controller" };
        var response = new ProfileDataResponse();
        using var cancellation = new CancellationTokenSource();
        var backend = new Mock<IProfileBackendClient>(MockBehavior.Strict);
        backend.Setup(client => client.SearchProfilesAsync(request, clientId, cancellation.Token))
            .ReturnsAsync((response, (HttpStatusCode?)null, (string?)null, (string?)null));
        var context = new DefaultHttpContext { RequestAborted = cancellation.Token };
        if (clientId != null)
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("clientId", clientId) }, "Test"));
        var controller = new ProfileController(NullLogger<ProfileController>.Instance,
            new Mock<IDiagnosticContext>().Object, backend.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = context }
        };

        var result = Assert.IsType<OkObjectResult>(await controller.PostProfile(request));

        Assert.Same(response, result.Value);
        backend.Verify(client => client.SearchProfilesAsync(request, clientId, cancellation.Token), Times.Once);
        backend.VerifyNoOtherCalls();
    }

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
