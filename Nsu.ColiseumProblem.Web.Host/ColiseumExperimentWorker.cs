using Microsoft.Extensions.Hosting;

namespace Nsu.ColiseumProblem.Web.Host
{
    internal class ColiseumExperimentWorker : BackgroundService
    {
        private ColiseumSandbox _coliseumSandbox;
        private IHostApplicationLifetime _hostApplicationLifetime;
        public int ExperimentCount { set; get; }

        public ColiseumExperimentWorker(
            ColiseumSandbox coliseumSandbox,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _coliseumSandbox = coliseumSandbox;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int targetExperimentCount = 100;
            int experimentCount = 0;
            int successCount = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                bool success = await _coliseumSandbox.RunExperimentAsync();
                if (success)
                    successCount++;
                experimentCount++;

                if (experimentCount == targetExperimentCount)
                    _hostApplicationLifetime.StopApplication();
            }

            Console.WriteLine($"Number of experiments: {experimentCount}");
            Console.WriteLine($"Number of successes: {successCount}");
            Console.WriteLine($"Success rate: {(double)successCount / experimentCount * 100:00.0}%");
        }
    }
}