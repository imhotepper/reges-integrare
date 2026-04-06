using RegesIntegration.DTOs;
using RegesIntegration.Services;
using Xunit;

namespace RegesIntegration.Tests;

/// <summary>
/// Unit tests for DTO classes
/// </summary>
public class DtoTests
{
    [Fact]
    public void UserDTO_CanBeCreated()
    {
        // Arrange & Act
        var userDto = new UserDTO("testuser", "testpass");

        // Assert
        Assert.Equal("testuser", userDto.User);
        Assert.Equal("testpass", userDto.Password);
    }

    [Fact]
    public void Header_CanBeCreated()
    {
        // Arrange & Act
        var header = new Header
        {
            MessageId = "test-id",
            ClientApplication = "test-app",
            Version = "5",
            Operation = "TestOperation",
            User = "TestUser"
        };

        // Assert
        Assert.Equal("test-id", header.MessageId);
        Assert.Equal("test-app", header.ClientApplication);
        Assert.Equal("5", header.Version);
        Assert.Equal("TestOperation", header.Operation);
        Assert.Equal("TestUser", header.User);
    }

    [Fact]
    public void MessageResponse_CanBeCreated()
    {
        // Arrange & Act
        var response = new MessageResponse
        {
            ResponseId = "response-123",
            Header = new Header { MessageId = "msg-123" }
        };

        // Assert
        Assert.Equal("response-123", response.ResponseId);
        Assert.NotNull(response.Header);
        Assert.Equal("msg-123", response.Header.MessageId);
    }

    [Fact]
    public void MessageResult_CanBeCreated()
    {
        // Arrange & Act
        var result = new MessageResult
        {
            ResponseId = "response-456",
            Header = new Header { MessageId = "msg-456" },
            Result = new Result
            {
                Code = "SUCCESS",
                Description = "Operation completed successfully"
            }
        };

        // Assert
        Assert.Equal("response-456", result.ResponseId);
        Assert.NotNull(result.Header);
        Assert.NotNull(result.Result);
        Assert.Equal("SUCCESS", result.Result.Code);
        Assert.Equal("Operation completed successfully", result.Result.Description);
    }
}

/// <summary>
/// Unit tests for Utils class
/// </summary>
public class UtilsTests
{
    [Fact]
    public void GetArgumentValue_ReturnsCorrectValue()
    {
        // Arrange
        var args = new[] { "--type", "send-salariat", "--user", "testuser" };

        // Act
        var type = Utils.GetArgumentValue(args, "type");
        var user = Utils.GetArgumentValue(args, "user");

        // Assert
        Assert.Equal("send-salariat", type);
        Assert.Equal("testuser", user);
    }

    [Fact]
    public void GetArgumentValue_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var args = new[] { "--type", "send-salariat" };

        // Act
        var password = Utils.GetArgumentValue(args, "password");

        // Assert
        Assert.Null(password);
    }

    [Fact]
    public void GetUserFromArguments_ThrowsException_WhenUserMissing()
    {
        // Arrange
        var args = new[] { "--password", "testpass" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Utils.GetUserFromArguments(args));
    }

    [Fact]
    public void GetUserFromArguments_ThrowsException_WhenPasswordMissing()
    {
        // Arrange
        var args = new[] { "--user", "testuser" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Utils.GetUserFromArguments(args));
    }

    [Fact]
    public void GetUserFromArguments_ReturnsUserDTO_WhenBothPresent()
    {
        // Arrange
        var args = new[] { "--user", "testuser", "--password", "testpass" };

        // Act
        var userDto = Utils.GetUserFromArguments(args);

        // Assert
        Assert.NotNull(userDto);
        Assert.Equal("testuser", userDto.User);
        Assert.Equal("testpass", userDto.Password);
    }
}