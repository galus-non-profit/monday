using System.Reflection;
using FluentValidation;
using Hangfire;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Monday.WebApi.Auth;
using Monday.WebApi.Behaviors;
using Monday.WebApi.Commands;
using Monday.WebApi.Database;
using Monday.WebApi.Database.Interfaces;
using Monday.WebApi.Handlers;
using Monday.WebApi.Interfaces;
using Monday.WebApi.Options;
using Monday.WebApi.Query;
using Monday.WebApi.Services;
using Monday.WebApi.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSeq();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//builder.Services.AddOptions<SqlOptions>().Configure(options =>
//{
//    options.SqlConnectionString = "Server=(local)\\SQLExpress;Database=Potato;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";
//});

var options = new SqlOptions() { SqlConnectionString = "Server=(local)\\SQLExpress;Database=Potato;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False" };

builder.Services.AddSingleton(options);

builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));

builder.Services
    .AddProblemDetails(options =>
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Extensions.Add("request", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
        });

builder.Services.AddExceptionHandler<ExceptionsHandler>();

builder.Services.AddHangfireServer();

builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IDBEngine, SqlServerEngine>();

var assembly = Assembly.GetExecutingAssembly();

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(assembly);
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AddBookBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AddUserBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(DeleteUserBehavior<,>));
    }
);

builder.Services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IBookReadService, BookReadService>();

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserReadService, UserReadService>();

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

app.MapPost("/users", async ([FromServices] ISender mediator, [FromBody] AddUser command, CancellationToken cancellationToken = default) => await mediator.Send(command, cancellationToken))
    .WithName("AddUser")
    .WithOpenApi();

app.MapDelete("/users", async ([FromServices] ISender mediator, [FromBody] DeleteUser command, CancellationToken cancellationToken = default) => await mediator.Send(command, cancellationToken))
    .WithName("DeleteUser")
    .WithOpenApi();

app.MapGet("/users", async ([FromServices] ISender mediator, CancellationToken cancellationToken = default) => await mediator.Send(new GetUsers(), cancellationToken))
    .WithOpenApi();

app.MapHub<BookHub>("/bookHub");
app.MapHub<UserHub>("/userHub");
app.MapHub<NotificationsHub>("/notificationsHub");

app.Run();
