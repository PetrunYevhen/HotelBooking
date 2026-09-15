import { apiClient } from "@/shared/lib/api-client"
import type { HotelierHotel } from "@/domains/accommodations/api/hotelier"
import type { HotelierApplication } from "@/domains/auth/api/auth"

export const getApplications = async (status = "Pending") => (await apiClient.get<HotelierApplication[]>("/api/admin/hotelier-applications", { params: { status } })).data
export const approveApplication = (applicationId: string) => apiClient.post(`/api/admin/hotelier-applications/${applicationId}/approve`)
export const rejectApplication = (applicationId: string, reason: string) => apiClient.post(`/api/admin/hotelier-applications/${applicationId}/reject`, { reason })
export const getProperties = async () => (await apiClient.get<HotelierHotel[]>("/api/admin/properties")).data
export const assignPropertyOwner = (hotelId: string, ownerUserId: string | null) => apiClient.put(`/api/admin/properties/${hotelId}/owner`, { ownerUserId })
