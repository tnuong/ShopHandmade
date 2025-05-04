using back_end.Core.Models;
using back_end.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace back_end.Configuration
{
    public class ServerStartupTask : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ServerStartupTask> _logger;

        public ServerStartupTask(IServiceScopeFactory serviceScopeFactory, ILogger<ServerStartupTask> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MyStoreDbContext>();
            await dbContext.Database.ExecuteSqlRawAsync(
                "UPDATE AspNetUsers SET IsOnline = 0, RecentOnlineTime = GETDATE()",
                cancellationToken: cancellationToken
            );
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
