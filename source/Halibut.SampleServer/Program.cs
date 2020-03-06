using System;
using System.Security.Cryptography.X509Certificates;
using Halibut.SampleContracts;
using Halibut.ServiceModel;
using Serilog;

namespace Halibut.SampleServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            Console.Title = "Halibut Server";
            var certificate = new X509Certificate2("extender.pfx");
            var services = new DelegateServiceFactory();
            services.Register<ICalculatorService>(() => new CalculatorService());

            using (var server = new HalibutRuntime(services, certificate))
            {
                server.Poll(new Uri("poll://SQ-EXTENDER"), new ServiceEndPoint(new Uri("https://localhost:8434"), "177B1A7194EC6C8A80F5666243E5021DF2DC03C2"));
                
                Console.WriteLine("Server listening on port 8433. Type 'exit' to quit, or 'cls' to clear...");
                while (true)
                {
                    var line = Console.ReadLine();
                    if (string.Equals("cls", line, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Clear();
                        continue;
                    }

                    if (string.Equals("q", line, StringComparison.OrdinalIgnoreCase))
                        return;

                    if (string.Equals("exit", line, StringComparison.OrdinalIgnoreCase))
                        return;

                    Console.WriteLine("Unknown command. Enter 'q' to quit.");
                }
            }
        }
    }
}