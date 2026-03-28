namespace DTO.ValueObjects;

public record Money(
    decimal TotalPrice,
    string Currency);