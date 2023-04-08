using System.Collections.Concurrent;
using System.Text;
using RabbitMQ.Client.Events;
using Shared.Data;
using Shared.Services.Contract;

namespace WorkerService;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMessageBroker _messageBroker;

    private Thread _thread0;
    private Thread _thread1;
    private Thread _thread2;
    private Thread _thread3;

    private readonly BlockingCollection<BasicDeliverEventArgs> _message0 = new();
    private readonly BlockingCollection<BasicDeliverEventArgs> _message1 = new();
    private readonly BlockingCollection<BasicDeliverEventArgs> _message2 = new();
    private readonly BlockingCollection<BasicDeliverEventArgs> _message3 = new();

    private Stack<int> _index = new();

    public Worker(IMessageBroker messageBroker, IServiceScopeFactory serviceScopeFactory)
    {
        _messageBroker = messageBroker;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _messageBroker.Dispose();

        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _thread0 = new Thread(Thread0);
        _thread1 = new Thread(Thread1);
        _thread2 = new Thread(Thread2);
        _thread3 = new Thread(Thread3);

        _thread0.Start();
        _thread1.Start();
        _thread2.Start();
        _thread3.Start();

        _messageBroker.Subscribe("hashes");
        _messageBroker.MessageReceivedEvent += MessageReceivedEvent;

        //return Task.CompletedTask;
    }


    private void MessageReceivedEvent(object sender, BasicDeliverEventArgs e)
    {
        var index = GetIndex();

        switch (index)
        {
            case 0:
                _message0.Add(e);
                break;
            case 1:
                _message1.Add(e);
                break;
            case 2:
                _message1.Add(e);
                break;
            case 3:
                _message2.Add(e);
                break;
        }
    }

    private void Thread0()
    {
        while (true)
        {
            var message = _message0.Take();
            Process(message);
        }
    }

    private void Thread1()
    {
        while (true)
        {
            var message = _message1.Take();
            Process(message);
        }
    }

    private void Thread2()
    {
        while (true)
        {
            var message = _message2.Take();
            Process(message);
        }
    }

    private void Thread3()
    {
        while (true)
        {
            var message = _message3.Take();
            Process(message);
        }
    }

    private int GetIndex()
    {
        if (_index.Count == 0)
        {
            for (int i = 0; i < 4; i++)
                _index.Push(i);
        }

        return _index.Pop();
    }

    private async Task Process(BasicDeliverEventArgs e)
    {
        try
        {
            var ea = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(ea);

            Hashes hashes = new()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now.Date,
                RandomStringSha1 = message,
            };

            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<HashesDbContext>();
            await dbContext.Hashes.AddAsync(hashes);
            int result = await dbContext.SaveChangesAsync();

            if (result > 0)
                _messageBroker.Acknowledge(e);
        }
        catch (Exception)
        {
            throw;
        }
    }


}