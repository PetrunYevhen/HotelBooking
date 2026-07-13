import { apiClient } from "../lib/api-client"

export interface CreateBookingRequest {
    hotelId: string
    roomId: string
    userId: string
    checkIn: string
    checkOut: string
    guestCount: number
    firstName: string
    lastName: string
    email: string
    phoneNumber: string
    specialRequest?: string
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
    createdAt: string
}

export async function getBookingsByUserId(userId: string): Promise<BookingDto[]> {
    const { data } = await apiClient.get<BookingDto[]>(`/api/bookings/user/${userId}`)
    return data
}
