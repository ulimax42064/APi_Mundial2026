using MongoDB.Driver;
using Microsoft.Extensions.Options;
using TupApi.Config;
using TupApi.Middleware;
using TupApi.Repositories;
using TupApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "TUP Mundial 2026 API",
        Version     = "v1",
        Description = "API REST para gestión de partidos y tickets del Mundial 2026"
    });
});

// MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Repositorios (capa de datos)
builder.Services.AddScoped<IPartidoRepository, PartidoRepository>();
builder.Services.AddScoped<ITicketRepository,  TicketRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Servicios (capa de negocio)
builder.Services.AddScoped<PartidoService>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<UsuarioService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Middleware global de errores — debe ir primero
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors("AllowAll");

// Swagger siempre visible (útil para la entrega)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TUP Mundial 2026 API v1");
    c.RoutePrefix = "swagger";
});

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
