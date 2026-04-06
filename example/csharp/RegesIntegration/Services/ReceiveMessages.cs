using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;
using RegesIntegration.DTOs;

namespace RegesIntegration.Services;

/// <summary>
/// Service for receiving message results from REGES API
/// </summary>
public class ReceiveMessages
{
    private readonly HttpClient _httpClient;
    private readonly Utils _utils;
    private readonly string _receiveFolderName;

    public ReceiveMessages(HttpClient httpClient, Utils utils, string receiveFolderName = "receive")
    {
        _httpClient = httpClient;
        _utils = utils;
        _receiveFolderName = receiveFolderName;
    }

    /// <summary>
    /// Start receiving messages from API queue
    /// </summary>
    public async Task ReceiveAsync(UserDTO user, string loginDomain, string apiDomain, int maxMessages)
    {
        string outPath = Path.Combine(Directory.GetCurrentDirectory(), _receiveFolderName);

        if (!CheckRequirements(outPath))
        {
            return;
        }

        string token = await _utils.LoginAsync(user, loginDomain);
        await GetMessagesAsync(token, apiDomain, outPath, 0, maxMessages);
    }

    /// <summary>
    /// Recursively get messages from the API queue
    /// </summary>
    private async Task GetMessagesAsync(string token, string apiDomain, string outPath, int count, int maxMessages)
    {
        string url = $"{apiDomain}/api/Status/PollMessage";

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        request.Content = new StringContent("", Encoding.UTF8, "application/xml");

        try
        {
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string responseXml = await response.Content.ReadAsStringAsync();
                var messageResult = DeserializeXml<MessageResult>(responseXml);

                // Save message to file
                string fileName = $"response-{messageResult.Header?.MessageId}.xml";
                string filePath = Path.Combine(outPath, fileName);
                await File.WriteAllTextAsync(filePath, responseXml);

                count++;
                Console.WriteLine($"Scris: {count}  {messageResult.Header?.MessageId}");

                if (count < maxMessages)
                {
                    // Wait before next request
                    await Task.Delay(1000);
                    await GetMessagesAsync(token, apiDomain, outPath, count, maxMessages);
                }
                else
                {
                    Console.WriteLine("Limita max-message a fost atinsa, rulati iar");
                }
            }
            else
            {
                Console.WriteLine("Nu exista mesaje pentru consum in acest moment");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Eroare la primirea mesajelor: {ex.Message}");
        }
    }

    /// <summary>
    /// Check if required directory exists
    /// </summary>
    private bool CheckRequirements(string outPath)
    {
        if (!Directory.Exists(outPath))
        {
            Console.WriteLine($"Folderul {_receiveFolderName} nu exista in directorul curent");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Deserialize XML string to object
    /// </summary>
    private T DeserializeXml<T>(string xml) where T : class
    {
        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(xml);
        return (T)serializer.Deserialize(reader)!;
    }
}
