using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;
using RegesIntegration.DTOs;

namespace RegesIntegration.Services;

/// <summary>
/// Service for sending employee (Salariat) messages to REGES API
/// </summary>
public class SendSalariatMessages
{
    private readonly HttpClient _httpClient;
    private readonly Utils _utils;
    private readonly string _inFolderName;
    private readonly string _outFolderName;

    public SendSalariatMessages(HttpClient httpClient, Utils utils, string inFolderName = "IN-SALARIAT", string outFolderName = "OUT")
    {
        _httpClient = httpClient;
        _utils = utils;
        _inFolderName = inFolderName;
        _outFolderName = outFolderName;
    }

    /// <summary>
    /// Start processing employee XML files
    /// </summary>
    public async Task StartAsync(UserDTO user, string loginDomain, string apiDomain)
    {
        string inPath = Path.Combine(Directory.GetCurrentDirectory(), _inFolderName);
        string outPath = Path.Combine(Directory.GetCurrentDirectory(), _outFolderName);

        if (!CheckRequirements(inPath, outPath))
        {
            return;
        }

        var files = Directory.GetFiles(inPath, "*.xml");

        if (files.Length == 0)
        {
            Console.WriteLine("Nu exista fisiere XML in folderul IN-SALARIAT");
            return;
        }

        string token = await _utils.LoginAsync(user, loginDomain);
        Console.WriteLine($"Total fisiere: {files.Length}");

        int count = 0;
        foreach (var file in files)
        {
            try
            {
                string xml = await File.ReadAllTextAsync(file);
                var response = await SendMessageAsync(token, xml, apiDomain);

                // Save response
                string responseFileName = $"response-sal-{response.Header?.MessageId}.xml";
                string responseFilePath = Path.Combine(outPath, responseFileName);
                SaveResponseToXml(response, responseFilePath);

                // Move processed file
                string destFile = Path.Combine(outPath, Path.GetFileName(file));
                File.Move(file, destFile, true);

                Console.WriteLine($"Am finalizat: {response.ResponseId}");
                Console.WriteLine($"Am procesat: {++count} din {files.Length}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la procesarea fisierului {Path.GetFileName(file)}: {ex.Message}");
            }
        }

        Console.WriteLine("Procesare finalizata!");
    }

    /// <summary>
    /// Send employee message to API
    /// </summary>
    private async Task<MessageResponse> SendMessageAsync(string token, string xml, string apiDomain)
    {
        string url = $"{apiDomain}/api/Salariat";

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        request.Content = new StringContent(xml, Encoding.UTF8, "application/xml");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseXml = await response.Content.ReadAsStringAsync();
        return DeserializeXml<MessageResponse>(responseXml);
    }

    /// <summary>
    /// Check if required directories exist
    /// </summary>
    private bool CheckRequirements(string inPath, string outPath)
    {
        if (!Directory.Exists(inPath))
        {
            Console.WriteLine($"Folderul {_inFolderName} nu exista in directorul curent");
            return false;
        }

        if (!Directory.Exists(outPath))
        {
            Console.WriteLine($"Folderul {_outFolderName} nu exista in directorul curent");
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

    /// <summary>
    /// Save response object to XML file
    /// </summary>
    private void SaveResponseToXml(MessageResponse response, string filePath)
    {
        var serializer = new XmlSerializer(typeof(MessageResponse));
        using var writer = new StreamWriter(filePath);
        serializer.Serialize(writer, response);
    }
}
