using System.IO;
using System.Net;
using System.Threading.Tasks;
using ClientClassLibrary;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace TestProject1;

public class SomethingApiConsumerTestsUsingWireMock
{
    [Fact]
    public async Task GetSomething_WhenTheTesterSomethingExists_ReturnsTheSomething()
    {
        // Arrange
        var server = WireMockServer.Start();
        server
            .Given(Request.Create()
                .UsingGet()
                .WithPath("/tester")
                .WithParam("q1", "test")
                .WithParam("q2", "ok")
                .WithHeader("Accept", "application/json")
            )
            .WithTitle("Something API Consumer-Something API")
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBodyAsJson(new
                    {
                        Id = "{{request.PathSegments.[0]}}",
                        FirstName = "Totally",
                        LastName = "Awesome"
                    })
                    .WithTransformer()
            );

        // Act
        var client = RestEase.RestClient.For<ISomethingApi>(server.Urls[0]);

        var something = await client.GetSomethingAsync("tester", "test", "ok");

        // Assert
        Assert.Equal("tester", something.Id);

        // Save mapping to folder
        server.SaveStaticMappings(Path.Combine("..", "..", "..", "wiremock-mappings"));
    }
}