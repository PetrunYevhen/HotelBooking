using BuildingBlock.Domain;
using Users.Domain.Enums;
using Users.Domain.ValueObjects;

namespace Users.Domain.Entities;

public class User : Entity
{
    public UserId  UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public UserPersonalInfo PersonalInfo { get; set; }
    public Role Role { get; set; } = Role.Unknown;
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiresAt { get; private set; }
}