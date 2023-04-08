using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Services.Concrete;
using Shared.Services.Contract;
using WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IMessageBroker, MessageBroker>();
        services.AddDbContext<HashesDbContext>(x => x.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));
    })
    .Build();

await host.RunAsync();
