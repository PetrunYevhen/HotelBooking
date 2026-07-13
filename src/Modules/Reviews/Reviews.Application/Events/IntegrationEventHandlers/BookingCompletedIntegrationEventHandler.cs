using Bookings.IntegrationEvents;
using MediatR;
using Reviews.Domain.Entities.EligibleReview;
using Reviews.Domain.RepositoryContract;

namespace Reviews.Application.Events.IntegrationEventHandlers;

public class BookingCompletedIntegrationEventHandler 
    : INotificationHandler<BookingCompletedIntegrationEvent>
{
    private readonly IEligibleReviewRepository _eligibleReviewRepository;

    public BookingCompletedIntegrationEventHandler(IEligibleReviewRepository eligibleReviewRepository)
    {
        _eligibleReviewRepository = eligibleReviewRepository;
    }

    public async Task Handle(BookingCompletedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var existing = await _eligibleReviewRepository.GetByBookingIdAsync(notification.BookingId, cancellationToken);
        if (existing is not null)
            return;
        
        var eligible = EligibleReview.Create(notification.HotelId, notification.BookingId,
            notification.UserId);
        
        await _eligibleReviewRepository.AddAsync(eligible, cancellationToken);
    }
}