using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.RepositoryContracts;

namespace Users.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UsersDbContext _usersDbContext;

    public UserRepository(UsersDbContext usersDbContext)
    {
        _usersDbContext = usersDbContext;
    }

    public Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken) =>
        _usersDbContext.Users.FirstOrDefaultAsync(user => user.UserId == userId, cancellationToken);

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken) =>
        _usersDbContext.Users.FirstOrDefaultAsync(user => user.Username == username, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        _usersDbContext.Users.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash, CancellationToken cancellationToken) =>
        _usersDbContext.Users.FirstOrDefaultAsync(user => user.RefreshTokenHash == refreshTokenHash, cancellationToken);

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        await _usersDbContext.Users.AddAsync(user, cancellationToken);
        return user;
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        _usersDbContext.Users.Update(user);
        return Task.CompletedTask;
    }
}
