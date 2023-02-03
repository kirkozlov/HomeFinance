using HomeFinanace.DataAccess.Core.DBModels;
using HomeFinance.DataAccess.EFBasic;
using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Services;
using HomeFinance.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace HomeFinanceApi.BackgroundWorker
{
    public class RepeatableExecution : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer? _timer = null;

        public RepeatableExecution(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            lock (_timer!)
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IRepetableService>();
                service.FindAndExcecuteRepeatableOperation();
            }
           
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
