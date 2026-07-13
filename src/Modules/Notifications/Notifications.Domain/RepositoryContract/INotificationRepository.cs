using Notifications.Domain.Entities;

namespace Notifications.Domain.RepositoryContract;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken);
}