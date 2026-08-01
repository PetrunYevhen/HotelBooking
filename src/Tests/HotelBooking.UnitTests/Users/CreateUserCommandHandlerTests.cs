using Users.Application.Command.Users.CreateUser;
using Users.Domain.Entities;
using Users.Domain.RepositoryContracts;
using Users.Application.Services;
using Xunit;

namespace HotelBooking.UnitTests.Users;

public sealed class CreateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithUniqueValidData_AddsUserAndReturnsId()
    {
        var repository = new InMemoryUserRepository();
        var handler = new CreateUserCommandHandler(repository, new TestPasswordHashingService());

        var result = await handler.Handle(ValidCommand(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(repository.Users);
        Assert.Equal(result.Value, repository.Users.Single().UserId.Value);
        Assert.Equal("yevhen@example.com", repository.Users.Single().Email);
    }

    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ReturnsFailureWithoutAddingUser()
    {
        var repository = new InMemoryUserRepository();
        var handler = new CreateUserCommandHandler(repository, new TestPasswordHashingService());
        await handler.Handle(ValidCommand(), CancellationToken.None);

        var result = await handler.Handle(ValidCommand("yevhen-2", "YEVHEN@EXAMPLE.COM"), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("User.EmailAlreadyExists", result.Error.Code);
        Assert.Single(repository.Users);
    }

    private static CreateUserCommand ValidCommand(
        string username = "yevhen",
        string email = "yevhen@example.com") => new()
    {
        Username = username,
        Password = "a secure password",
        Email = email,
        FirstName = "Yevhen",
        LastName = "Petrun",
        PhoneNumber = "+380501234567"
    };

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
