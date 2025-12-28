using Oracle.ManagedDataAccess.Client;
using SolicitudesAPI.Repositories;
using SolicitudesAPI.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurar conexión a Oracle
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
    return new OracleConnection(connectionString);
});

// Inyección de dependencias
builder.Services.AddScoped<ISolicitudRepository, SolicitudRepository>();
builder.Services.AddScoped<ISolicitudService, SolicitudService>();

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();