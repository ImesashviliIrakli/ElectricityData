
using Enitites.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repositories.DownloadDataRepositories;
using Repositories.ElectricityRepositories;
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

#region LocalVariables
var log = builder.Logging.Services.BuildServiceProvider().GetRequiredService<ILogger<DownloadDataRepository>>();
var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;
var context = new AppDbContext(dbContextOptions);
#endregion

builder.Services.AddScoped<IElectricityRepository, ElectricityRepository>();
builder.Services.AddTransient<IDownloadDataRepository, DownloadDataRepository>();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await Download(log, context);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task Download(ILogger<DownloadDataRepository> log, AppDbContext context)
{
    
    var repository = new DownloadDataRepository(context, log);

    await repository.DownloadAll();

}