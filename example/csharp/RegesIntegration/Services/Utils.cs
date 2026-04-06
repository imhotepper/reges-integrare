using System.Net.Http.Json;
using RegesIntegration.DTOs;

namespace RegesIntegration.Services;

/// <summary>
/// Utility class for authentication and common operations
/// </summary>
public class Utils
{
    private readonly HttpClient _httpClient;

    public Utils(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get user credentials from command line arguments
    /// </summary>
    public static UserDTO GetUserFromArguments(string[] args)
    {
        string? user = GetArgumentValue(args, "user");
        string? password = GetArgumentValue(args, "password");

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("User and password must be provided");
        }

        return new UserDTO(user, password);
    }

    /// <summary>
    /// Login to the REGES API using OAuth2 password grant
    /// </summary>
    public async Task<string> LoginAsync(UserDTO user, string loginDomain)
    {
        var tokenUrl = $"{loginDomain}/realms/API/protocol/openid-connect/token";

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", "reges-api" },
            { "client_secret", "FjtrYvDTGZKiyHGdSWymOvxhqifTJ7Em" },
            { "username", user.User },
            { "password", user.Password }
        });

        var response = await _httpClient.PostAsync(tokenUrl, content);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

        if (tokenResponse?.AccessToken == null)
        {
            throw new Exception("Failed to obtain access token");
        }

        return tokenResponse.AccessToken;
    }

    /// <summary>
    /// Get value of a command line argument
    /// </summary>
    public static string? GetArgumentValue(string[] args, string key)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals($"--{key}", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                return args[i + 1];
            }
        }
        return null;
    }
}
