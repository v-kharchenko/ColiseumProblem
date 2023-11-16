using MassTransit;
using Nsu.ColiseumProblem.Strategy;
using System.Reflection;

namespace Nsu.ColiseumProblem.MassTransit.Player
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddScoped<ICardPickStrategy, WinningCardStrategy>();
            builder.Services.AddSingleton<DeckManager>();
            
            string queue = builder.Configuration.GetValue<string>("player") ?? "public";
            queue += "-queue";
            
            builder.Services.AddMassTransit(x =>
                {
                    x.SetKebabCaseEndpointNameFormatter();

                    var assembly = typeof(Program).Assembly;
                    x.AddConsumers(assembly);

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host("localhost", "/", h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });

                        cfg.ReceiveEndpoint(queue, ep =>
                        {
                            ep.ConfigureConsumer<PickCardConsumer>(context);
                            ep.ConfigureConsumer<CardPickedConsumer>(context);
                        });
                    });
                });

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}