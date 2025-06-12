using Booking.Infrastructure.DI;
using RoomManagment.Infrastructure.DI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Register the module services
builder.Services.AddBookingModule(builder.Configuration);
builder.Services.AddRoomModule(builder.Configuration);

// Register logging
builder.Host.UseSerilog((сontext, configuration) => configuration
    .ReadFrom.Configuration(сontext.Configuration));
builder.Logging.ClearProviders().AddConsole();

builder.Services.AddHttpLogging(logg =>
{
    logg.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties
                         | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpLogging();
}

app.MapControllers();

app.Run();