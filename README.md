# HotelBooking

Modular ASP.NET Core hotel-booking application with a React client, PostgreSQL,
MailHog and Stripe/stripe-mock payment adapters.

## Local configuration

Tracked configuration intentionally contains no database password or live Stripe
key. Copy `.env.example` to `.env` for Docker Compose. For a direct API run, set:

```bash
export ConnectionStrings__DefaultConnection='Host=localhost;Port=5433;Database=hotel_booking;Username=postgres;Password=your-local-password'
export Stripe__ApiBase='http://localhost:12111'
export Stripe__ApiKey='sk_test_mock'
export Stripe__IsMock='true'
```

Never commit `.env`, connection strings, SMTP credentials or Stripe secret keys.

## Run

```bash
docker compose up --build
```

The frontend is available at `http://localhost:5173`, API Swagger at
`http://localhost:8080/swagger`, MailHog at `http://localhost:8025`, and API
health at `http://localhost:8080/health` (adjust `API_PORT` if needed).

## Database migrations

Each module owns its DbContext and migrations. Apply migrations explicitly from
the `src` directory before the first direct API run:

```bash
dotnet ef database update --project Modules/Accommodations/Accommodations.Infrastructure --startup-project API/HotelBooking.API --context AccommodationsDbContext
dotnet ef database update --project Modules/Bookings/Bookings.Infrastructure --startup-project API/HotelBooking.API --context BookingDbContext
dotnet ef database update --project Modules/Payments/Payments.Infrastructure --startup-project API/HotelBooking.API --context PaymentsDbContext
dotnet ef database update --project Modules/Reviews/Reviews.Infrastructure --startup-project API/HotelBooking.API --context ReviewsDbContext
dotnet ef database update --project Modules/Notifications/Notifications.Infrastructure --startup-project API/HotelBooking.API --context NotificationsDbContext
dotnet ef database update --project Modules/Users/Users.Infrastructure --startup-project API/HotelBooking.API --context UsersDbContext
```

The booking migration installs PostgreSQL `btree_gist`; the database user needs
permission to create that extension.

## Verification

```bash
dotnet test src/HotelBooking.sln --no-restore --disable-build-servers -m:1
npm --prefix client run build
npm --prefix client run lint
```

Test-suite structure and purpose are documented in `src/Tests/README.md`.
