import { apiClient } from "../lib/api-client"

export interface HotelDto {
    hotelId: string
    name: string
    city: string
    country: string
    rating: number | null
    minRoomPriceAmount: number | null
    minRoomPriceCurrency: string | null
}

export async function getAllHotels(): Promise<HotelDto[]> {
    const { data } = await apiClient.get<HotelDto[]>("/api/hotels")
    return data
}

export interface HotelDetailsDto {
    name: string
    description: string
    rating: number | null
    minRoomPriceAmount: number | null
    minRoomPriceCurrency: string | null
    country: string
    city: string
    street: string
    postalCode: string
}

export interface FacilityDto {
    name: string
    category: string
}

export async function getHotelById(id: string): Promise<HotelDetailsDto> {
    const { data } = await apiClient.get<HotelDetailsDto>(`/api/hotels/${id}`)
    return data
}

export async function getHotelFacilities(id: string): Promise<FacilityDto[]> {
    const { data } = await apiClient.get<FacilityDto[]>(`/api/hotels/${id}/facilities`)
    return data
}

export interface SetHotelPoliciesRequest {
    cancellationPolicyType: string
    deadlineDays: number | null
    percentagePenalty: number | null
    petPolicy: string
    smokingPolicy: string
    checkOutHoursPolicy: number
}

export async function setHotelPolicies(hotelId: string, payload : SetHotelPoliciesRequest): Promise<void>{
    await apiClient.put(`/api/hotels/${hotelId}/policies`, payload)
}