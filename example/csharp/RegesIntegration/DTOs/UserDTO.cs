namespace RegesIntegration.DTOs;

/// <summary>
/// Data Transfer Object for user credentials
/// </summary>
public class UserDTO
{
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public UserDTO() { }

    public UserDTO(string user, string password)
    {
        User = user;
        Password = password;
    }
}
