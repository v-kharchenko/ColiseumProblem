
using Nsu.ColiseumProblem.Strategy;

namespace Nsu.ColiseumProblem.Web.Player
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddScoped<ICardPickStrategy, WinningCardStrategy>();

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}