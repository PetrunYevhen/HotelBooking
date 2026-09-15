import { apiClient, setAccessToken } from "@/shared/lib/api-client"

interface AuthResponse { accessToken: string }

export interface SignUpRequest {
    username: string
    password: string
    email: string
    firstName: string
    lastName: string
    phoneNumber: string
    intent: "guest" | "list-property"
    businessApplication?: BusinessApplicationRequest
}

export interface BusinessApplicationRequest {
    legalBusinessName: string
    registrationNumber: string
    taxNumber?: string
    businessEmail: string
    businessPhoneNumber: string
    firstPropertyName: string
    firstPropertyAddress: string
}

export interface UserProfile { userId: string; username: string; email: string; firstName: string; lastName: string; phoneNumber: string; role: string }
export interface HotelierApplication { hotelierApplicationId: string; status: "Pending" | "Approved" | "Rejected"; legalBusinessName: string; registrationNumber: string; taxNumber?: string; businessEmail: string; businessPhoneNumber: string; firstPropertyName: string; firstPropertyAddress: string; submittedAt: string; reviewedAt?: string; rejectionReason?: string }

export async function signIn(usernameOrEmail: string, password: string): Promise<void> {
    const { data } = await apiClient.post<AuthResponse>("/api/auth/signin", { usernameOrEmail, password })
    setAccessToken(data.accessToken)
}

export async function signUp(request: SignUpRequest): Promise<void> {
    const { data } = await apiClient.post<AuthResponse>("/api/auth/signup", request)
    setAccessToken(data.accessToken)
}

export const getMe = async () => (await apiClient.get<UserProfile>("/api/auth/me")).data
export const getHotelierApplication = async () => (await apiClient.get<HotelierApplication | null>("/api/auth/hotelier-application")).data
export const submitHotelierApplication = (request: BusinessApplicationRequest) => apiClient.post("/api/auth/hotelier-application", request)
