namespace RoomManagment.Application.Dto;

public class RoomBookingDetailsDto
{
    public string Title = "Details";
    public string ImageUrl { get; set; } = string.Empty;
    public string HotelName { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
}