using SharedKernel.Contracts;
using Microsoft.EntityFrameworkCore;
using Accommodations.Domain.Entities.HotelierApplications;
using Accommodations.Domain.RepositoryContract.HotelierApplications;

namespace Accommodations.Infrastructure.Repositories;

public sealed class HotelierApplicationRepository(AccommodationsDbContext dbContext) : IHotelierApplicationRepository
{
    public Task<HotelierApplication?> GetByIdAsync(HotelierApplicationId applicationId, CancellationToken cancellationToken) =>
        dbContext.HotelierApplications.FirstOrDefaultAsync(x => x.HotelierApplicationId == applicationId, cancellationToken);

    public Task<HotelierApplication?> GetLatestByApplicantAsync(AccountId applicantId, CancellationToken cancellationToken) =>
        dbContext.HotelierApplications.Where(x => x.ApplicantId == applicantId).OrderByDescending(x => x.SubmittedAt)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<bool> HasPendingForApplicantAsync(AccountId applicantId, CancellationToken cancellationToken) =>
        dbContext.HotelierApplications.AnyAsync(x => x.ApplicantId == applicantId && x.Status == HotelierApplicationStatus.Pending, cancellationToken);

    public Task AddAsync(HotelierApplication application, CancellationToken cancellationToken) => dbContext.HotelierApplications.AddAsync(application, cancellationToken).AsTask();

    public Task UpdateAsync(HotelierApplication application, CancellationToken cancellationToken)
    {
        dbContext.HotelierApplications.Update(application);
        return Task.CompletedTask;
    }
}
