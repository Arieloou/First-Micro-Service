using Microsoft.EntityFrameworkCore;
using NotificationsService.AppDomain.Interfaces;
using NotificationsService.Application.Interfaces;
using NotificationsService.Application.Services;
using NotificationsService.Infraestructure.NotificationsProvider;
using NotificationsService.Infraestructure.Persistence;
using NotificationsService.Infraestructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationConnection")));

// Registra el servicio de Aplicación Principal
builder.Services.AddScoped<NotificationAppService>();

// Registra la implementación del Repositorio
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddTransient<EmailProvider>();
builder.Services.AddTransient<SMSProvider>();

builder.Services.AddTransient<Func<string, INotificationProvider>>
    (serviceProvider => key =>
    {
        switch (key.ToLowerInvariant())
        {
            case "email":
                return serviceProvider.GetRequiredService<EmailProvider>();
            case "sms":
                return serviceProvider.GetRequiredService<SMSProvider>();
            default:
                throw new NotSupportedException($"El proveedor '{key}' no está soportado.");
        }
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Código Generado por Google AiStudio
    Thread.Sleep(5000); // 5 segundos de gracia adicionales

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        try
        {
            // Aplica cualquier migración pendiente para crear la BD y las tablas
            dbContext.Database.Migrate();
            Console.WriteLine("Migración de la base de datos completada exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrió un error durante la migración de la base de datos: {ex.Message}");
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Endpoint de Health Check simple para verificar el estado del microservicio (Supervisarlo)
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
