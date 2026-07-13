# HotelBooking test strategy

The `Tests` directory is a sibling of `Modules`. This keeps test code outside production modules while mirroring the modular-monolith structure.

## Test projects

- `HotelBooking.UnitTests` verifies domain rules and value objects in memory. These tests must be fast, deterministic and must not use a database, network, clock-dependent delays or module startup.
- `HotelBooking.IntegrationTests` verifies that production infrastructure components work together. The initial suite builds every EF Core/Npgsql model, which catches invalid mappings, missing entity configurations and provider-specific model errors without requiring a running database.
- `HotelBooking.ArchitectureTests` turns architectural decisions into executable rules. It prevents domain projects from referencing application/infrastructure layers or the domain of another business module.

## Why all three are needed

Unit tests answer: "Is this business rule correct?" Integration tests answer: "Do our components and adapters fit together?" Architecture tests answer: "Does the codebase still follow the intended modular design?" None of these categories replaces another.

## Recommended next levels

1. Add PostgreSQL repository tests with Testcontainers. Run real migrations and verify constraints, transactions, optimistic concurrency, inbox/outbox idempotency and Dapper queries against the same database engine used in production.
2. Add API/component tests with `WebApplicationFactory` after authentication and module startup can be configured for tests. Verify routing, serialization, authorization, validation and HTTP status mappings.
3. Add a small number of end-to-end tests for the highest-value journeys: search room -> create booking -> complete payment -> confirm booking -> check-in -> check-out -> review. Keep them few because they are slow and expensive to diagnose.
4. Add contract tests for integration events. They protect event names and payload compatibility between modules independently of implementation details.
5. Collect coverage in CI, but use it as a risk indicator rather than a target by itself. Prioritize booking/payment state transitions, pricing, cancellation policies and inbox/outbox failure paths.

## Commands

```bash
dotnet test src/HotelBooking.sln
dotnet test src/Tests/HotelBooking.UnitTests/HotelBooking.UnitTests.csproj
dotnet test src/Tests/HotelBooking.IntegrationTests/HotelBooking.IntegrationTests.csproj
dotnet test src/Tests/HotelBooking.ArchitectureTests/HotelBooking.ArchitectureTests.csproj
dotnet test src/HotelBooking.sln --collect:"XPlat Code Coverage"
```
