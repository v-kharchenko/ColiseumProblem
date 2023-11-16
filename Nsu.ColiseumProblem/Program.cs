using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nsu.ColiseumProblem.Database;
using Nsu.ColiseumProblem.Sandbox;
using Nsu.ColiseumProblem.Strategy;

namespace Nsu.ColiseumProblem
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHost(args).Run();
        }

        public static IHost CreateHost(string[] args)
        {
            int experimentCount = 100;

            IHostBuilder builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ColiseumExperimentWorker>();
                services.AddScoped<ColiseumSandbox>();
                services.AddScoped<CardMaster>();
                
                services.AddScoped<IDeckShufller, FisherYatesDeckShufller>();

                services.AddScoped<ICardPickStrategy, WinningCardStrategy>();
                services.AddTransient<Player>();
                services.AddDbContext<ColiseumContext>(options => options.UseSqlite(GetConnectionString()));
            });

            IHost host = builder.Build();

            return host;
        }

        private static string GetConnectionString()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            return "Data Source=" + Path.Join(path, "coliseum.db");
        }
    }
}