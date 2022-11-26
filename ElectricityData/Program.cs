using Contracts;
using DownloadService;
using Enitites.Data;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Serilog;
using Serilog.Formatting.Json;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region LoggerServices
string path = @"c:\temp\logs\ElectricityData_logs.json";

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path: "appSettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.File(new JsonFormatter(), path, shared: true)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
#endregion

#region DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
#endregion

builder.Services.AddScoped<IElectricityRepository, ElectricityRepository>();
builder.Services.AddTransient<IDownloadManager, DownloadManager>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

#region Create instance of DownloadDataRepository

var dbcontext = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
var log = app.Services.GetRequiredService<ILogger<DownloadManager>>();
var electricity = app.Services.CreateScope().ServiceProvider.GetRequiredService<IElectricityRepository>();
var repository = new DownloadManager(dbcontext, log, electricity);
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await repository.DownloadAll();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();