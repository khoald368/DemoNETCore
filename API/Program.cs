using System.IO.Compression;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Common Configs
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting();
builder.Services.AddMvc(o =>
{
    o.EnableEndpointRouting = false;
});
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Config Response Compression
builder.Services.AddResponseCompression();
builder.Services.Configure<GzipCompressionProviderOptions>(o =>
{
    o.Level = CompressionLevel.Fastest;
});

//Config Database
builder.Services.AddEntityFrameworkSqlServer();
builder.Services.AddDbContext<AppDbContext>((s, o) =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), c => c.CommandTimeout(300));
    o.UseInternalServiceProvider(s);
    o.EnableDetailedErrors(true);
    o.EnableSensitiveDataLogging(true);
});


// Config App Methods
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
