using Users.Application.Command.Users.CreateAdmin;
using Users.Domain.Entities;
using Users.Domain.Enums;
using Users.Domain.RepositoryContracts;
using Users.Application.Services;
using Xunit;

namespace HotelBooking.UnitTests.Users;

public sealed class CreateAdminCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidData_CreatesAdministratorThroughDomainTransition()
    {
        var repository = new InMemoryUserRepository();
        var handler = new CreateAdminCommandHandler(repository, new TestPasswordHashingService());

        var result = await handler.Handle(new CreateAdminCommand
        {
            Username = "admin",
            Password = "a secure password",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "User",
            PhoneNumber = "+380501234567"
        }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(repository.Users);
        Assert.Equal(Role.Admin, repository.Users.Single().Role);
    }

    private sealed class InMemoryUserRepository : IUserRepository
    {
        public List<User> Users { get; } = [];

        public Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken) =>
            Task.FromResult(Users.SingleOrDefault(user => user.UserId == userId));

        public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken) =>
            Task.FromResult(Users.SingleOrDefault(user => user.Username == username));

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
            Task.FromResult(Users.SingleOrDefault(user => user.Email == email));

        public Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash, CancellationToken cancellationToken) =>
            Task.FromResult(Users.SingleOrDefault(user => user.RefreshTokenHash == refreshTokenHash));

        public Task<User> AddAsync(User user, CancellationToken cancellationToken)
        {
            Users.Add(user);
            return Task.FromResult(user);
        }

        public Task UpdateAsync(User user, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed class TestPasswordHashingService : IPasswordHashingService
    {
        public string Hash(string password) => $"hash:{password}";
        public bool Verify(string password, string encodedHash) => encodedHash == Hash(password);
    }
}
