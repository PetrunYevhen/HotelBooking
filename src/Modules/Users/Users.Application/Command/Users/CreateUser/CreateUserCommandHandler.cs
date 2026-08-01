using BuildingBlock.Domain;
using MediatR;
using Users.Domain.Entities;
using Users.Domain.RepositoryContracts;
using Users.Domain.ValueObjects;
using Users.Application.Services;

namespace Users.Application.Command.Users.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashingService _passwordHashingService;

    public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHashingService passwordHashingService)
    {
        _userRepository = userRepository;
        _passwordHashingService = passwordHashingService;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length is < 12 or > 128)
            return Result.Failure<Guid>(new Error("User.InvalidPassword", "Password must be between 12 and 128 characters."));

        var personalInfoResult = UserPersonalInfo.Create(request.FirstName, request.LastName, request.PhoneNumber);
        if (personalInfoResult.IsFailure)
            return Result.Failure<Guid>(personalInfoResult.Error);

        var userResult = User.Create(
            request.Username,
            _passwordHashingService.Hash(request.Password),
            request.Email,
            personalInfoResult.Value);
        if (userResult.IsFailure)
            return Result.Failure<Guid>(userResult.Error);

        var username = userResult.Value.Username;
        if (await _userRepository.GetByUsernameAsync(username, cancellationToken) is not null)
        {
            return Result.Failure<Guid>(new Error(
                "User.UsernameAlreadyExists",
                "A user with this username already exists."));
        }

        var email = userResult.Value.Email;
        if (await _userRepository.GetByEmailAsync(email, cancellationToken) is not null)
        {
            return Result.Failure<Guid>(new Error(
                "User.EmailAlreadyExists",
                "A user with this email address already exists."));
        }

        var user = await _userRepository.AddAsync(userResult.Value, cancellationToken);
        return Result.Success(user.UserId.Value);
    }
}
