using Autofac;
using Autofac.Extensions.DependencyInjection;
using HotelBooking.API.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

var apiConfig = new ApiStartup();

// Autofac modules
builder.Host.ConfigureContainer<ContainerBuilder>(apiConfig.ConfigureContainer);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Register logging
builder.Host.UseSerilog((сontext, configuration) => configuration
    .ReadFrom.Configuration(сontext.Configuration));

builder.Services.AddHttpLogging(logg =>
{
    logg.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties
                         | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
});

var app = builder.Build();

var autofacContainer = app.Services.GetAutofacRoot();

var apiConfigForInit = new ApiStartup();
apiConfigForInit.InitializeModules(autofacContainer);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpLogging();
}

app.MapControllers();

app.Run();

