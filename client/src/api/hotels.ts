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

export interface SearchHotelsParams {
    destination?: string
    checkIn?: string
    checkOut?: string
    guests?: number
    rooms?: number
}

export async function searchHotels(params: SearchHotelsParams): Promise<HotelDto[]> {
    const { data } = await apiClient.get<HotelDto[]>("/api/hotels/search", { params })
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
    id?: string
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

export interface HotelAddOnDto {
    hotelAddOnId: string
    hotelId: string
    code: string
    name: string
    description: string | null
    priceAmount: number
    priceCurrency: string
    pricingType: 1 | 2 | 3
    isActive: boolean
}

export async function getHotelAddOns(hotelId: string): Promise<HotelAddOnDto[]> {
    const { data } = await apiClient.get<HotelAddOnDto[]>(`/api/hotels/${hotelId}/add-ons`)
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
