using FluentValidation;
using Hangfire;
using MediatR;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Monday.WebApi.Behaviors;
using Monday.WebApi.Commands;
using Monday.WebApi.Handlers;
using Monday.WebApi.Interfaces;
using Monday.WebApi.Services;
using Monday.WebApi.SignalR.Hubs;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSeq();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));

builder.Services
    .AddProblemDetails(options =>
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Extensions.Add("request", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
        });

builder.Services.AddExceptionHandler<ExceptionsHandler>();

builder.Services.AddHangfireServer();

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AddBookBehavior<,>));
    }
);

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IBookReadService, BookReadService>();

builder.Services.AddSignalR()
    .AddMessagePackProtocol(options =>
    {
        options.SerializerOptions = MessagePackSerializerOptions.Standard
            .WithSecurity(MessagePackSecurity.UntrustedData);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHangfireDashboard();

app.MapPost("/books", async ([FromServices] ISender mediator, [FromBody] AddBook command, CancellationToken cancellationToken = default) => await mediator.Send(command, cancellationToken))
    .WithName("AddBook")
    .WithOpenApi();

app.MapHub<BookHub>("/bookHub");

app.Run();
