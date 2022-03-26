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
    private readonly WireMockServer _server;

    public SomethingApiConsumerTestsUsingWireMock()
    {
        _server = WireMockServer.Start();
    }

    [Fact]
    public async Task GetSomething_WhenTheTesterSomethingExists_ReturnsTheSomething()
    {
        // Arrange
        _server
            .Given(Request.Create()
                .UsingGet()
                .WithPath("/tester")
                .WithHeader("Accept", "application/json")
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBodyAsJson(new
                    {
                        Id = "tester",
                        FirstName = "Totally",
                        LastName = "Awesome"
                    })
            );

        // Act
        var client = RestEase.RestClient.For<ISomethingApi>(_server.Urls[0]);

        var something = await client.GetSomethingAsync("tester");

        // Assert
        Assert.Equal("tester", something.Id);
    }
}