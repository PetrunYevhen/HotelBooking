# ============ BASE IMAGE ============
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80
EXPOSE 443

# ============ BUILD IMAGE ============
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копіюємо тільки csproj файли для кешування restore
COPY ["API/HotelBooking.API/HotelBooking.API.csproj", "API/HotelBooking.API/"]
COPY ["Modules/Bookings/Bookings.Application/Bookings.Application.csproj", "Modules/Bookings/Bookings.Application/"]
COPY ["Modules/Bookings/Bookings.Domain/Bookings.Domain.csproj", "Modules/Bookings/Bookings.Domain/"]
COPY ["Modules/Bookings/Bookings.Infrastructure/Bookings.Infrastructure.csproj", "Modules/Bookings/Bookings.Infrastructure/"]
COPY ["Modules/Bookings/Bookings.IntegrationEvents/Bookings.IntegrationEvents.csproj", "Modules/Bookings/Bookings.IntegrationEvents/"]
COPY ["Modules/Accommodations/Accommodations.Application/Accommodations.Application.csproj", "Modules/Accommodations/Accommodations.Application/"]
COPY ["Modules/Accommodations/Accommodations.Domain/Accommodations.Domain.csproj", "Modules/Accommodations/Accommodations.Domain/"]
COPY ["Modules/Accommodations/Accommodations.Infrastructure/Accommodations.Infrastructure.csproj", "Modules/Accommodations/Accommodations.Infrastructure/"]
COPY ["Modules/Accommodations/Accommodations.IntegrationEvent/Accommodations.IntegrationEvent.csproj", "Modules/Accommodations/Accommodations.IntegrationEvent/"]
COPY ["Modules/Payments/Payments.Application/Payments.Application.csproj", "Modules/Payments/Payments.Application/"]
COPY ["Modules/Payments/Payments.Domain/Payments.Domain.csproj", "Modules/Payments/Payments.Domain/"]
COPY ["Modules/Payments/Payments.Infrastructure/Payments.Infrastructure.csproj", "Modules/Payments/Payments.Infrastructure/"]
COPY ["Modules/Payments/Payments.IntegrationEvents/Payments.IntegrationEvents.csproj", "Modules/Payments/Payments.IntegrationEvents/"]
COPY ["Modules/Notifications/Notifications.Application/Notifications.Application.csproj", "Modules/Notifications/Notifications.Application/"]
COPY ["Modules/Notifications/Notifications.Domain/Notifications.Domain.csproj", "Modules/Notifications/Notifications.Domain/"]
COPY ["Modules/Notifications/Notifications.Infrastructure/Notifications.Infrastructure.csproj", "Modules/Notifications/Notifications.Infrastructure/"]
COPY ["Modules/Notifications/Notifications.IntegrationEvents/Notifications.IntegrationEvents.csproj", "Modules/Notifications/Notifications.IntegrationEvents/"]
COPY ["Modules/Reviews/Reviews.Application/Reviews.Application.csproj", "Modules/Reviews/Reviews.Application/"]
COPY ["Modules/Reviews/Reviews.Domain/Reviews.Domain.csproj", "Modules/Reviews/Reviews.Domain/"]
COPY ["Modules/Reviews/Reviews.Infrastructure/Reviews.Infrastructure.csproj", "Modules/Reviews/Reviews.Infrastructure/"]
COPY ["Modules/Reviews/Reviews.IntegrationEvents/Reviews.IntegrationEvents.csproj", "Modules/Reviews/Reviews.IntegrationEvents/"]
COPY ["Common/BuildingBlocks/Application/Application.csproj", "Common/BuildingBlocks/Application/"]
COPY ["Common/BuildingBlocks/Domain/Domain.csproj", "Common/BuildingBlocks/Domain/"]
COPY ["Common/BuildingBlocks/Infrastructure/Infrastructure.csproj", "Common/BuildingBlocks/Infrastructure/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
COPY ["Directory.Packages.props", "./"]
COPY ["Directory.Build.props", "./"]

# Restore packages
RUN dotnet restore "API/HotelBooking.API/HotelBooking.API.csproj"

# Копіюємо решту файлів і збираємо
COPY . .
WORKDIR "/src/API/HotelBooking.API"
RUN dotnet build "HotelBooking.API.csproj" -c Release -o /app/build

# ============ PUBLISH IMAGE ============
FROM build AS publish
RUN dotnet publish "HotelBooking.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ============ FINAL IMAGE ============
FROM runtime AS final
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "HotelBooking.API.dll"]
