using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ClientClassLibrary;
using PactNet;
using Xunit;

namespace TestProject1;

public class SomethingApiConsumerTestsUsingPactV3
{
    private readonly IPactBuilderV3 _pactBuilder;

    public SomethingApiConsumerTestsUsingPactV3()
    {
        // Use default pact directory ..\..\pacts and default log directory ..\..\logs
        var pact = Pact.V3("Something API Consumer", "Something API V3");

        _pactBuilder = pact.UsingNativeBackend();
    }

    [Fact]
    public async Task GetSomething_WhenTheTesterSomethingExists_ReturnsTheSomething()
    {
        // Arrange
        _pactBuilder
            .UponReceiving("A GET request to retrieve the something")
            .Given("There is a something with id 'tester'")
            .WithRequest(HttpMethod.Get, "/tester")
            .WithHeader("Accept", "application/json")
            .WithQuery("q1", "test")
            .WithQuery("q2", "ok")

            .WillRespond()
            .WithStatus(HttpStatusCode.OK)
            .WithHeader("Content-Type", "application/json; charset=utf-8")
            .WithJsonBody(new
            {
                Id = "tester",
                FirstName = "Totally",
                LastName = "Awesome"
            });

        await _pactBuilder.VerifyAsync(async ctx =>
        {
            // Act
            var client = RestEase.RestClient.For<ISomethingApi>(ctx.MockServerUri);

            var something = await client.GetSomethingAsync("tester", "test", "ok");

            // Assert
            Assert.Equal("tester", something.Id);
        });
    }
}