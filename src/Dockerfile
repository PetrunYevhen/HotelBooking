# ============ BASE IMAGE ============
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# ============ BUILD IMAGE ============
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копіюємо тільки csproj файли для кешування restore
COPY ["API/HotelBooking.API/HotelBooking.API.csproj", "API/HotelBooking.API/"]
COPY ["Modules/BookingManagement/Booking.Application/Booking.Application.csproj", "Modules/BookingManagement/Booking.Application/"]
COPY ["Modules/BookingManagement/Booking.Domain/Booking.Domain.csproj", "Modules/BookingManagement/Booking.Domain/"]
COPY ["Modules/BookingManagement/Booking.Infrastructure/Booking.Infrastructure.csproj", "Modules/BookingManagement/Booking.Infrastructure/"]
COPY ["Modules/BookingManagement/Booking.IntegrationEvents/Booking.IntegrationEvents.csproj", "Modules/BookingManagement/Booking.IntegrationEvents/"]
COPY ["Modules/RoomManagement/RoomManagement.Application/RoomManagement.Application.csproj", "Modules/RoomManagement/RoomManagement.Application/"]
COPY ["Modules/RoomManagement/RoomManagement.Domain/RoomManagement.Domain.csproj", "Modules/RoomManagement/RoomManagement.Domain/"]
COPY ["Modules/RoomManagement/RoomManagement.Infrastructure/RoomManagement.Infrastructure.csproj", "Modules/RoomManagement/RoomManagement.Infrastructure/"]
COPY ["Modules/RoomManagement/RoomManagement.IntegrationEvent/RoomManagement.IntegrationEvent.csproj", "Modules/RoomManagement/RoomManagement.IntegrationEvent/"]
COPY ["Modules/HotelManagement/HotelManagement.Application/HotelManagement.Application.csproj", "Modules/HotelManagement/HotelManagement.Application/"]
COPY ["Modules/HotelManagement/HotelManagement.Domain/HotelManagement.Domain.csproj", "Modules/HotelManagement/HotelManagement.Domain/"]
COPY ["Modules/HotelManagement/HotelManagement.Infrastructure/HotelManagement.Infrastructure.csproj", "Modules/HotelManagement/HotelManagement.Infrastructure/"]
COPY ["Modules/HotelManagement/HotelManagement.IntegrationEvent/HotelManagement.IntegrationEvent.csproj", "Modules/HotelManagement/HotelManagement.IntegrationEvent/"]
COPY ["Common/BuildingBlocks/Application/Application.csproj", "Common/BuildingBlocks/Application/"]
COPY ["Common/BuildingBlocks/Domain/Domain.csproj", "Common/BuildingBlocks/Domain/"]
COPY ["Common/BuildingBlocks/Infrastructure/Infrastructure.csproj", "Common/BuildingBlocks/Infrastructure/"]
COPY ["Modules/SharedKernel/SharedKernel/SharedKernel.csproj", "Modules/SharedKernel/SharedKernel/"]
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
FROM base AS final
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "HotelBooking.API.dll"]
