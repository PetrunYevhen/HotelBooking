using BuildingBlock.Domain;
using Users.Domain.Enums;

namespace Users.Domain.Entities;

public class User : Entity
{
    public UserId  UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public Role Role { get; set; } =  Role.Unknown;
}