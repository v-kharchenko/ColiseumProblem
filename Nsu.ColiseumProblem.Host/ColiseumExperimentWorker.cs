using Microsoft.Extensions.Hosting;
using Nsu.ColiseumProblem.Sandbox;

namespace Nsu.ColiseumProblem
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
            int targetExperimentCount = 100000;
            int experimentCount = 0;
            int successCount = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                bool success = _coliseumSandbox.RunExperiment();
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