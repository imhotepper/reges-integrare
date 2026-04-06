using RegesIntegration.DTOs;
using RegesIntegration.Services;

namespace RegesIntegration;

/// <summary>
/// Main console application for REGES API integration
/// Supports sending employee/contract messages and receiving responses
/// </summary>
class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            // Parse command line arguments
            string? type = Utils.GetArgumentValue(args, "type");
            string? user = Utils.GetArgumentValue(args, "user");
            string? password = Utils.GetArgumentValue(args, "password");
            string? loginDomain = Utils.GetArgumentValue(args, "loginDomain");
            string? apiDomain = Utils.GetArgumentValue(args, "apiDomain");
            string? maxMessageStr = Utils.GetArgumentValue(args, "max-message");

            // Validate required arguments
            if (string.IsNullOrEmpty(user))
            {
                Console.WriteLine("parametrul --user nu este trimis");
                PrintUsage();
                return 1;
            }

            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("parametrul --password nu este trimis");
                PrintUsage();
                return 1;
            }

            if (string.IsNullOrEmpty(type))
            {
                Console.WriteLine("parametrul --type nu este trimis");
                PrintUsage();
                return 1;
            }

            if (string.IsNullOrEmpty(loginDomain))
            {
                Console.WriteLine("parametrul --loginDomain nu este trimis");
                PrintUsage();
                return 1;
            }

            if (string.IsNullOrEmpty(apiDomain))
            {
                Console.WriteLine("parametrul --apiDomain nu este trimis");
                PrintUsage();
                return 1;
            }

            var userDto = new UserDTO(user, password);
            var httpClient = new HttpClient();
            var utils = new Utils(httpClient);

            // Process based on type
            switch (type.ToLower())
            {
                case "send-salariat":
                    var sendSalariat = new SendSalariatMessages(httpClient, utils);
                    await sendSalariat.StartAsync(userDto, loginDomain, apiDomain);
                    break;

                case "send-contract":
                    var sendContract = new SendContractMessages(httpClient, utils);
                    await sendContract.StartAsync(userDto, loginDomain, apiDomain);
                    break;

                case "receive":
                    if (string.IsNullOrEmpty(maxMessageStr) || !int.TryParse(maxMessageStr, out int maxMessages))
                    {
                        Console.WriteLine("parametrul --max-message nu este trimis sau nu este un numar valid");
                        PrintUsage();
                        return 1;
                    }

                    var receiveMessages = new ReceiveMessages(httpClient, utils);
                    await receiveMessages.ReceiveAsync(userDto, loginDomain, apiDomain, maxMessages);
                    break;

                default:
                    Console.WriteLine($"parametrul --type nu este valid: {type}");
                    Console.WriteLine("Valori valide: send-salariat, send-contract, receive");
                    PrintUsage();
                    return 1;
            }

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Detalii: {ex.InnerException.Message}");
            }
            return 1;
        }
    }

    static void PrintUsage()
    {
        Console.WriteLine("\nUtilizare:");
        Console.WriteLine("  dotnet run -- --type <tip> --user <utilizator> --password <parola> --loginDomain <domeniu-login> --apiDomain <domeniu-api> [--max-message <numar>]");
        Console.WriteLine("\nParametri:");
        Console.WriteLine("  --type          : Tipul operatiei (send-salariat, send-contract, receive)");
        Console.WriteLine("  --user          : Utilizator API");
        Console.WriteLine("  --password      : Parola API");
        Console.WriteLine("  --loginDomain   : Domeniul de autentificare (ex: https://login.dev.inspectiamuncii.org)");
        Console.WriteLine("  --apiDomain     : Domeniul API (ex: https://api.dev.inspectiamuncii.org)");
        Console.WriteLine("  --max-message   : Numarul maxim de mesaje de primit (doar pentru receive)");
        Console.WriteLine("\nExemple:");
        Console.WriteLine("  dotnet run -- --type send-salariat --user myuser --password mypass --loginDomain https://login.dev.inspectiamuncii.org --apiDomain https://api.dev.inspectiamuncii.org");
        Console.WriteLine("  dotnet run -- --type receive --user myuser --password mypass --loginDomain https://login.dev.inspectiamuncii.org --apiDomain https://api.dev.inspectiamuncii.org --max-message 10");
    }
}
