import { apiClient } from "@/lib/api-client"
import type { FacilityDto, HotelAddOnDto, SetHotelPoliciesRequest } from "./hotels"

export interface HotelierHotel { hotelId: string; name: string; city: string; country: string; ownerUserId: string | null }
export interface HotelierRoom { roomId: string; hotelId: string; roomNumber: string; type: string; beds: number; capacity: number; status: string }
export interface HotelierBooking { id: string; hotelId: string; roomId: string; roomNumber: string; guestName: string; guestEmail: string; checkInDate: string; checkOutDate: string; totalPrice: number; currency: string; guestsCount: number; status: string; createdAt: string }
export interface HotelierOverview { arrivalsToday: number; departuresToday: number; activeStays: number; newBookings: number; nextActions: HotelierBooking[] }
export interface HotelierCalendar { rooms: HotelierRoom[]; occupancy: HotelierBooking[] }
export interface HotelierSettings { rooms: HotelierRoom[]; hotelFacilities: FacilityDto[]; roomFacilities: Record<string, FacilityDto[]>; addOns: HotelAddOnDto[] }

export const getHotelierHotels = async () => (await apiClient.get<HotelierHotel[]>("/api/hotelier/hotels")).data
export const assignHotelOwner = (hotelId: string, ownerUserId: string | null) => apiClient.put(`/api/hotelier/hotels/${hotelId}/owner`, { ownerUserId })
export const getOverview = async (hotelId: string) => (await apiClient.get<HotelierOverview>(`/api/hotelier/hotels/${hotelId}/overview`)).data
export const getHotelierBookings = async (hotelId: string, params: { from?: string; to?: string; status?: string; roomId?: string }) => (await apiClient.get<HotelierBooking[]>(`/api/hotelier/hotels/${hotelId}/bookings`, { params })).data
export const getCalendar = async (hotelId: string, from: string) => (await apiClient.get<HotelierCalendar>(`/api/hotelier/hotels/${hotelId}/calendar`, { params: { from, days: 14 } })).data
export const getSettings = async (hotelId: string) => (await apiClient.get<HotelierSettings>(`/api/hotelier/hotels/${hotelId}/settings`)).data
export const checkIn = (bookingId: string) => apiClient.post(`/api/hotelier/bookings/${bookingId}/checkin`)
export const checkOut = (bookingId: string) => apiClient.post(`/api/hotelier/bookings/${bookingId}/checkout`)
export const savePolicies = (hotelId: string, payload: SetHotelPoliciesRequest) => apiClient.put(`/api/hotelier/hotels/${hotelId}/policies`, payload)
export const addHotelAmenity = (hotelId: string, name: string) => apiClient.post(`/api/hotelier/hotels/${hotelId}/amenities`, [{ name, category: 0 }])
export const removeHotelAmenity = (hotelId: string, facilityId: string) => apiClient.delete(`/api/hotelier/hotels/${hotelId}/amenities/${facilityId}`)
export const addRoomAmenity = (hotelId: string, roomId: string, name: string) => apiClient.post(`/api/hotelier/hotels/${hotelId}/rooms/${roomId}/amenities`, [{ name, category: 0 }])
export const removeRoomAmenity = (hotelId: string, roomId: string, facilityId: string) => apiClient.delete(`/api/hotelier/hotels/${hotelId}/rooms/${roomId}/amenities/${facilityId}`)
export const createAddOn = (hotelId: string, payload: Omit<HotelAddOnDto, "hotelAddOnId" | "hotelId" | "isActive">) => apiClient.post(`/api/hotelier/hotels/${hotelId}/add-ons`, payload)
export const updateAddOn = (hotelId: string, addOnId: string, payload: Omit<HotelAddOnDto, "hotelAddOnId" | "hotelId" | "isActive">) => apiClient.put(`/api/hotelier/hotels/${hotelId}/add-ons/${addOnId}`, payload)
export const setAddOnActive = (hotelId: string, addOnId: string, active: boolean) => apiClient.post(`/api/hotelier/hotels/${hotelId}/add-ons/${addOnId}/${active ? "activate" : "deactivate"}`)
