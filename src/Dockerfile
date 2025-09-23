FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["API/HotelBooking.API/HotelBooking.API.csproj", "HotelBooking.API/"]
COPY ["Modules/BookingManagement/Booking.Application/Booking.Application.csproj", "Modules/Booking/Booking.Application/"]
COPY ["Modules/BookingManagement/Booking.Domain/Booking.Domain.csproj", "Modules/Booking/Booking.Domain/"]
COPY ["Modules/BookingManagement/Booking.Infrastructure/Booking.Infrastructure.csproj", "Modules/Booking/Booking.Infrastructure/"]
COPY ["Modules/BookingManagement/Booking.IntegrationEvents/Booking.IntegrationEvents.csproj", "Modules/Booking/Booking.IntegrationEvents/"]
COPY ["Modules/RoomManagement/RoomManagement.Application/RoomManagement.Application.csproj", "Modules/RoomManagement/Room.Application/"]
COPY ["Modules/RoomManagement/RoomManagement.Domain/RoomManagement.Domain.csproj", "Modules/RoomManagement/Room.Domain/"]
COPY ["Modules/RoomManagement/RoomManagement.Infrastructure/RoomManagement.Infrastructure.csproj", "Modules/RoomManagement/Room.Infrastructure/"]
COPY ["Common/BuildingBlocks/Application/Application.csproj", "Common/BuildingBlocks/Application/"]
COPY ["Common/BuildingBlocks/Domain/Domain.csproj", "Common/BuildingBlocks/Domain/"]
COPY ["Common/BuildingBlocks/Infrastructure/Infrastructure.csproj", "Common/BuildingBlocks/Infrastructure/"]
COPY ["Modules/HotelManagement/HotelManagement.Application/HotelManagement.Application.csproj", "Modules/HotelManagement/HotelManagement.Application/"]
COPY ["Modules/HotelManagement/HotelManagement.Domain/HotelManagement.Domain.csproj", "Modules/HotelManagement/HotelManagement.Domain/"]
COPY ["Modules/HotelManagement/HotelManagement.Infrastructure/HotelManagement.Infrastructure.csproj", "Modules/HotelManagement/HotelManagement.Infrastructure/"]
COPY ["Directory.Packages.props", "./"]

RUN dotnet restore "API/HotelBooking.API/HotelBooking.API.csproj"

COPY . .
WORKDIR "/src/API/HotelBooking.API"
RUN dotnet build "HotelBooking.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotelBooking.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

ADD entrypoint.sh /
ENTRYPOINT ["/bin/bash", "/entrypoint.sh"]