using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Content
{
    [DisallowConcurrentExecution]
    public class DemoJob : IJob
    {
        private readonly ILogger<DemoJob> _logger;
        public DemoJob(ILogger<DemoJob> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("進入排程程序{0}!", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return Task.CompletedTask;
        }
    }
}
