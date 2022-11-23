
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
string path = @"c:\temp\logs\studentPortal_Logs.json";

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

var log = builder.Logging.Services.BuildServiceProvider().GetRequiredService<ILogger<DownloadDataRepository>>();

var client = new HttpClient();
var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;
var context = new AppDbContext(dbContextOptions);

var repository = new DownloadDataRepository(client, context, log);

var taskList = new List<Task<bool>>(); 
repository.DownloadData("10763/2022-02");
repository.DownloadData("10764/2022-03");
repository.DownloadData("10765/2022-04");
repository.DownloadData("10766/2022-05");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
