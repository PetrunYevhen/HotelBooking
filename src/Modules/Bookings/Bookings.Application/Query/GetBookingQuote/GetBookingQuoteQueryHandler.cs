using Bookings.Application.Services.Quotes;
using BuildingBlock.Domain;
using MediatR;

namespace Bookings.Application.Query.GetBookingQuote;

public sealed class GetBookingQuoteQueryHandler : IRequestHandler<GetBookingQuoteQuery, Result<BookingQuoteDto>>
{
    private readonly IBookingQuoteService _quoteService;
    public GetBookingQuoteQueryHandler(IBookingQuoteService quoteService) => _quoteService = quoteService;

    public async Task<Result<BookingQuoteDto>> Handle(GetBookingQuoteQuery request, CancellationToken cancellationToken)
    {
        var quote = await _quoteService.GetQuoteAsync(new BookingQuoteRequest(request.HotelId, request.RoomId,
            request.CheckIn, request.CheckOut, request.GuestCount, request.AddOns ?? []), cancellationToken);
        if (quote.IsFailure)
            return Result.Failure<BookingQuoteDto>(quote.Error);

        var value = quote.Value;
        return Result.Success(new BookingQuoteDto
        {
            BaseTotal = value.BaseTotal.Amount,
            AddOnsTotal = value.Total.Amount - value.BaseTotal.Amount,
            Total = value.Total.Amount,
            Currency = value.Total.Currency,
            AddOns = value.AddOnLines.Select(line => new BookingQuoteAddOnDto
            {
                HotelAddOnId = line.HotelAddOnId,
                Code = line.Code,
                Name = line.Name,
                PricingType = line.PricingType,
                Quantity = line.Quantity,
                UnitPrice = line.UnitPrice.Amount,
                LineTotal = line.LineTotal.Amount,
                Currency = line.LineTotal.Currency
            }).ToList()
        });
    }
}
