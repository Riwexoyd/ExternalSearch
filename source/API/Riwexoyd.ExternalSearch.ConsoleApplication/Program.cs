using Riwexoyd.ExternalSearch.ConsoleApplication.Services;

namespace Riwexoyd.ExternalSearch.ConsoleApplication
{
    internal sealed class Program
    {
        private static async Task Main(string[] args)
        {
            using SearchingService searchingService = new();
            Console.WriteLine("Write /q to close application");
            CancellationTokenSource cancellationTokenSource = new();
            Console.CancelKeyPress += delegate {
                cancellationTokenSource.Cancel();
            };

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine("Enter game title:");
                // TODO : Ctrl + C should interrupt Console.ReadLine
                string? text = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(text))
                    continue;

                if (text == "/q")
                {
                    cancellationTokenSource.Cancel();
                    break;
                }

                string result = await searchingService.SearchAsync(text, cancellationTokenSource.Token);

                Console.WriteLine(result);
            }
        }
    }
}