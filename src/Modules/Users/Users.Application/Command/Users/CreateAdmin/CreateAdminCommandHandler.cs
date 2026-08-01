using BuildingBlock.Domain;
using MediatR;
using Users.Domain.Entities;
using Users.Domain.RepositoryContracts;
using Users.Domain.ValueObjects;
using Users.Application.Services;

namespace Users.Application.Command.Users.CreateAdmin;

public sealed class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashingService _passwordHashingService;

    public CreateAdminCommandHandler(IUserRepository userRepository, IPasswordHashingService passwordHashingService)
    {
        _userRepository = userRepository;
        _passwordHashingService = passwordHashingService;
    }

    public async Task<Result<Guid>> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
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

        var user = userResult.Value;
        var promotionResult = user.PromoteToAdmin();
        if (promotionResult.IsFailure)
            return Result.Failure<Guid>(promotionResult.Error);

        if (await _userRepository.GetByUsernameAsync(user.Username, cancellationToken) is not null)
        {
            return Result.Failure<Guid>(new Error(
                "User.UsernameAlreadyExists",
                "A user with this username already exists."));
        }

        if (await _userRepository.GetByEmailAsync(user.Email, cancellationToken) is not null)
        {
            return Result.Failure<Guid>(new Error(
                "User.EmailAlreadyExists",
                "A user with this email address already exists."));
        }

        await _userRepository.AddAsync(user, cancellationToken);
        return Result.Success(user.UserId.Value);
    }
}
