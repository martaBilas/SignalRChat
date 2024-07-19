using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SignalRChat.Controllers;

namespace ChatUnitTests;

public class ChatsControllerTests
{
    private readonly Mock<IChatService> _chatServiceMock;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly ChatsController _controller;

    public ChatsControllerTests()
    {
        _chatServiceMock = new Mock<IChatService>();
        _userServiceMock = new Mock<IUserService>();
        _controller = new ChatsController(_chatServiceMock.Object, _userServiceMock.Object);
    }

    [Fact]
    public void DeleteChat_WhenUserIsCreator_ShouldDeleteChat()
    {
        var chatId = 34;
        var creatorId = 123;

        _chatServiceMock.Setup(c => c.IsChatExist(chatId)).Returns(true);
        _userServiceMock.Setup(u => u.CheckIsUserCreator(creatorId, chatId)).Returns(true);

        var result = _controller.DeleteChat(new DeleteChatModel { ChatId = chatId, CreatorId = creatorId });

        _chatServiceMock.Verify(c => c.DeleteChat(chatId), Times.Once);
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Chat is deleted", (result as OkObjectResult)?.Value);
    }

    [Fact]
    public void DeleteChat_WhenUserIsNotCreator_ShouldThrowException()
    {
        var chatId = 21;
        var creatorId = 456;

        _chatServiceMock.Setup(c => c.IsChatExist(chatId)).Returns(true);
        _userServiceMock.Setup(u => u.CheckIsUserCreator(creatorId, chatId)).Returns(false);

        Assert.Throws<NotCreatorException>(() =>
            _controller.DeleteChat(new DeleteChatModel { ChatId = chatId, CreatorId = creatorId }));
    }
}
