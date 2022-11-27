using Contracts;
using DownloadService;
using Enitites.Data;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using NLog;
using Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region LoggerServices
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
"/nlog.config"));
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
#endregion

#region DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
#endregion

builder.Services.AddScoped<IElectricityRepository, ElectricityRepository>();
builder.Services.AddTransient<IDownloadManager, DownloadManager>();
builder.Services.AddControllers();
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
var log = app.Services.CreateScope().ServiceProvider.GetRequiredService<ILoggerManager>();
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