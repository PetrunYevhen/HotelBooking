using SharedKernel.Contracts;
using Accommodations.Domain.Entities.HotelierApplications;

namespace Accommodations.Domain.RepositoryContract.HotelierApplications;

public interface IHotelierApplicationRepository
{
    Task<HotelierApplication?> GetByIdAsync(HotelierApplicationId applicationId, CancellationToken cancellationToken);
    Task<HotelierApplication?> GetLatestByApplicantAsync(AccountId applicantId, CancellationToken cancellationToken);
    Task<bool> HasPendingForApplicantAsync(AccountId applicantId, CancellationToken cancellationToken);
    Task AddAsync(HotelierApplication application, CancellationToken cancellationToken);
    Task UpdateAsync(HotelierApplication application, CancellationToken cancellationToken);
}
