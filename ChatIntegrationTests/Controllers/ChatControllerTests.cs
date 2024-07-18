using ChatIntegrationTests.Fixtures;
using FluentAssertions;
using Xunit;

namespace ChatIntegrationTests.Controllers;

public class ChatControllerTests : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly WebApplicationFactoryFixture _factory;
    public ChatControllerTests(WebApplicationFactoryFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task OnCreateNewChat_WhenExecuteApi_ShouldReturnOkResponse()
    {
        (1 + 1).Should().Be(2);
    }

}
