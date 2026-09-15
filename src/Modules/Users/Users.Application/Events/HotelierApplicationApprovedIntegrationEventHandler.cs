using Infrastructure.EventBus;
using MediatR;
using SharedKernel.Contracts;
using Users.Domain.Entities;
using Users.Domain.Enums;
using Users.Domain.RepositoryContracts;

namespace Users.Application.Events;

public sealed class HotelierApplicationApprovedIntegrationEventHandler(IUserRepository userRepository)
    : INotificationHandler<ContractIntegrationEvent<HotelierApplicationApproved>>
{
    public async Task Handle(ContractIntegrationEvent<HotelierApplicationApproved> notification, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(notification.Data.ApplicantId), cancellationToken)
            ?? throw new InvalidOperationException("Applicant account was not found.");
        if (user.Role != Role.User) return;
        var result = user.ChangeRole(Role.Hotelier);
        if (result.IsFailure) throw new InvalidOperationException(result.Error.Code);
        await userRepository.UpdateAsync(user, cancellationToken);
    }
}
