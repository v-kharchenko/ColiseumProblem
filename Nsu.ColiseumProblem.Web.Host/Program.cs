using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nsu.ColiseumProblem.Database;
using Nsu.ColiseumProblem.Sandbox;
using Nsu.ColiseumProblem.Strategy;

namespace Nsu.ColiseumProblem.Web.Host
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHost(args).Run();
        }

        public static IHost CreateHost(string[] args)
        {
            IHostBuilder builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ColiseumExperimentWorker>();
                services.AddScoped<ColiseumSandbox>();
                services.AddScoped<PlayerClient>();
                services.AddScoped<CardMaster>();
                services.AddScoped<IDeckShufller, DeckLoadingShuffler>();
                services.AddScoped<ICardPickStrategy, WinningCardStrategy>();
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