using Microsoft.Extensions.DependencyInjection;
using TuringMachineSimulator.Interfaces;

public class Program
{
    public static async Task Main()
    {
        try
        {
            bool acceptWord = false;
            string stopState = string.Empty;

            IConfiguration configuration = ConfigureServices();
             
            string jsonMachine = GetJsonMachine();

            Simulator simulator = configuration.InitMachine(jsonMachine);

            (acceptWord, stopState) = simulator.InitSimulation();

            Console.Clear();

            Console.WriteLine(stopState);

            Console.WriteLine($"A palavra foi {(acceptWord ? "aceita" : "negada")}");

            Console.WriteLine("Deseja imprimir as computações? \n\r1 - Sim\n\r2 - Não");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                if (option == 1)
                    simulator.PrintComputations();
            }
            else
                throw new ArgumentOutOfRangeException("Entrada inválida. As computações não serão impressas.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    private static IConfiguration ConfigureServices()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddSingleton<IConfiguration, Configuration>();
        ServiceProvider provider = services.BuildServiceProvider();

        return provider.GetRequiredService<IConfiguration>();
    }

    private static string GetJsonMachine()
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string path = Path.Combine(basePath, "JsonTuringMachine.json");

        return File.ReadAllText(path);
    }
}