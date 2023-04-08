using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Services.Concrete;
using Shared.Services.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<IHashesRepository, HashesRepository>();
builder.Services.AddTransient<IStringGenerator, StringGenerator>();
builder.Services.AddSingleton<IMessageBroker, MessageBroker>();
builder.Services.AddDbContext<HashesDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
