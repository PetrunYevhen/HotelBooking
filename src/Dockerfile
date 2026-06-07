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
COPY ["Common/BuildingBlocks/Application/Application.csproj", "Common/BuildingBlocks/Application/"]
COPY ["Common/BuildingBlocks/Domain/Domain.csproj", "Common/BuildingBlocks/Domain/"]
COPY ["Common/BuildingBlocks/Infrastructure/Infrastructure.csproj", "Common/BuildingBlocks/Infrastructure/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
COPY ["Directory.Packages.props", "./"]

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
