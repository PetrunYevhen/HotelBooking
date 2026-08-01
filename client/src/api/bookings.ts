import { apiClient } from "../lib/api-client"

export interface CreateBookingRequest {
    hotelId: string
    roomId: string
    checkIn: string
    checkOut: string
    guestCount: number
    firstName: string
    lastName: string
    email: string
    phoneNumber: string
    specialRequest?: string
    addOns?: CreateBookingAddOnRequest[]
}

export interface CreateBookingAddOnRequest {
    hotelAddOnId: string
    quantity: number
}

export interface BookingQuoteRequest {
    hotelId: string
    roomId: string
    checkIn: string
    checkOut: string
    guestCount: number
    addOns?: CreateBookingAddOnRequest[]
}

export interface BookingQuoteAddOnDto {
    hotelAddOnId: string
    code: string
    name: string
    pricingType: number
    quantity: number
    unitPrice: number
    lineTotal: number
    currency: string
}

export interface BookingQuoteDto {
    baseTotal: number
    addOnsTotal: number
    total: number
    currency: string
    addOns: BookingQuoteAddOnDto[]
}

export async function getBookingQuote(payload: BookingQuoteRequest): Promise<BookingQuoteDto> {
    const { data } = await apiClient.post<BookingQuoteDto>("/api/bookings/quote", payload)
    return data
}

export async function createBooking(payload: CreateBookingRequest): Promise<string> {
    const { data } = await apiClient.post<string>("/api/bookings", payload)
    return data
}

export interface BookingDto {
    id: string
    roomId: string
    hotelId: string
    checkInDate: string
    checkOutDate: string
    totalPrice: number
    currency: string
    status: string
    guestsCount?: number
    completionReason?: "StaffCheckout" | "AutomaticCheckout" | null
    createdAt: string
    addOns?: BookingAddOnDto[]
}

export interface BookingAddOnDto {
    code: string
    name: string
    quantity: number
    unitPrice: number
    totalPrice: number
    currency: string
}

export async function getBookingById(bookingId: string): Promise<BookingDto> {
    const { data } = await apiClient.get<BookingDto>(`/api/bookings/${bookingId}`)
    return data
}

export async function getMyBookings(): Promise<BookingDto[]> {
    const { data } = await apiClient.get<BookingDto[]>("/api/bookings/me")
    return data
}

export async function cancelBooking(bookingId: string, reason?: string): Promise<void> {
    await apiClient.post(`/api/bookings/${bookingId}/cancel`, { reason })
}
