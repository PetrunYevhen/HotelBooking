import { apiClient } from "../lib/api-client"

export interface RoomDetailsDto {
    roomId: string
    hotelId: string
    roomNumber: string
    type: string
    beds: number
    capacity: number
    description: string | null
    status: string
    isActive: boolean
    basePriceAmount: number
    basePriceCurrency: string
    effectivePriceAmount: number
    effectivePriceCurrency: string
}

export async function getRoomsByHotelId(hotelId: string, checkIn: string): Promise<RoomDetailsDto[]> {
    const { data } = await apiClient.get<RoomDetailsDto[]>(`/api/rooms/${hotelId}/rooms`, {
        params: { checkIn },
    })
    return data
}
