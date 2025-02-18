using System.Text.Json;
using MediatR;
using Swashbuckle.AspNetCore.Filters;
using Zvonarev.FinBeat.Test.BusinessLogic;
using Zvonarev.FinBeat.Test.HttpDbLogging;
using Zvonarev.FinBeat.Test.Storage;
using Zvonarev.FinBeat.Test.Storage.UseCases.ApplyMigrations;
using Zvonarev.FinBeat.Test.WebApi.Middleware;

namespace Zvonarev.FinBeat.Test.WebApi;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var appConfig = builder.Configuration
            .Get<AppConfiguration>();

        // Add services to the container.

        builder.Services
            .AddLogging(o =>
            {
                //o.AddJsonConsole(x =>
                //{
                //    x.IncludeScopes = true;
                //    x.JsonWriterOptions = new JsonWriterOptions
                //    {
                //        Indented = true
                //    };
                //});
                o.AddConsole();
                o.AddDebug();
            })
            .AddTestFunctions()
            .AddSingleton(appConfig ?? throw new InvalidOperationException("Failed to load app configuration"))
            .AddStorage(appConfig.Storage)
            .AddHttpInfoLogging(appConfig.HttpLogging)
            .AddMediatR(x =>
            {
                x.Lifetime = ServiceLifetime.Scoped;
                x.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            })
            .AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.ExampleFilters();
        })
        .AddSwaggerExamplesFromAssemblies(typeof(Program).Assembly);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ErrorLoggingMiddleware>();

        app.UseApiRequestsLogging();
        app.UsePerRequestTransactions();

        app.UseAuthorization();


        app.MapControllers();

        //migrate db on start
        await using (var scope = app.Services.CreateAsyncScope())
        {
            var updateDbCommand = new UpdateDbCommand();
            await scope.ServiceProvider.GetRequiredService<IMediator>()
                .Send(updateDbCommand);
        }

        await app.RunAsync();
    }
}

