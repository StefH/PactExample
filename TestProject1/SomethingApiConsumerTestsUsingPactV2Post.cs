using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ClientClassLibrary;
using FluentAssertions;
using PactNet;
using Xunit;

namespace TestProject1;

public class SomethingApiConsumerTestsUsingPactV2Post
{
    private readonly IPactBuilderV2 _pactBuilder;

    public SomethingApiConsumerTestsUsingPactV2Post()
    {
        // Use default pact directory ..\..\pacts and default log directory ..\..\logs
        var pact = Pact.V2("Something API Consumer", "Something API V2 Post");

        _pactBuilder = pact.UsingNativeBackend();
    }

    [Fact]
    public async Task PostSomething_WhenTheTesterSomethingExists_ReturnsTheSomething()
    {
        // Arrange
        _pactBuilder
            .UponReceiving("A POST request to add the something")
            .Given("Post something")
            .WithRequest(HttpMethod.Post, "/tester")
            .WithHeader("Accept", "application/json")
            .WithJsonBody(new
            {
                DataNumber = 123,
                DataString = "str"
            })

            .WillRespond()
            .WithStatus(HttpStatusCode.OK)
            .WithHeader("Content-Type", "application/json; charset=utf-8")
            .WithJsonBody(new
            {
                Id = "1000",
                DataNumber = 123,
                DataString = "str"
            });

        await _pactBuilder.VerifyAsync(async ctx =>
        {
            // Act
            var client = RestEase.RestClient.For<ISomethingApi>(ctx.MockServerUri);

            var something = await client.PostSomethingAsync("tester", new
            {
                DataNumber = 123,
                DataString = "str"
            });

            // Assert
            something.Id.Should().Be("1000");
        });
    }
}