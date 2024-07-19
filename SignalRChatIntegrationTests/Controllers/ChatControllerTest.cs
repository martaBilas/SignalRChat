using ChatIntegrationTests.Fixtures;
using FluentAssertions;
using Infrastructure.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
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

        var newChatModel = new CreateChatModel
        {
            CreatorId = 1,
            Name = "Test Chat"
        };
        var client = _factory.CreateClient();

        var httpContent = new StringContent(JsonConvert.SerializeObject(newChatModel), Encoding.UTF8, "application/json");
        var request = await client.PostAsync("api/Chats/CreateNewChat", httpContent);

        request.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

}
