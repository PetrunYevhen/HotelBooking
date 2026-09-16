import { apiClient, clearAccessToken, readCsrfCookie, setAccessToken } from "@/shared/lib/api-client"

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

// Called once at app bootstrap (see main.tsx). The access token never persists between
// reloads (see api-client.ts), so this rehydrates it from the httpOnly refresh cookie
// when one is present - a signed-out/never-signed-in visitor has no CSRF cookie and
// this resolves immediately with no network round-trip.
export async function restoreSession(): Promise<void> {
    const csrfToken = readCsrfCookie()
    if (!csrfToken) {
        clearAccessToken()
        return
    }
    try {
        const { data } = await apiClient.post<AuthResponse>("/api/auth/refresh", null, {
            headers: { "X-CSRF-TOKEN": csrfToken },
        })
        setAccessToken(data.accessToken)
    } catch {
        clearAccessToken()
    }
}

export async function signOut(): Promise<void> {
    const csrfToken = readCsrfCookie()
    try {
        await apiClient.post("/api/auth/logout", null, csrfToken ? { headers: { "X-CSRF-TOKEN": csrfToken } } : undefined)
    } catch {
        // best-effort - still clear the local token even if the server call fails
    } finally {
        clearAccessToken()
    }
}

export const getMe = async () => (await apiClient.get<UserProfile>("/api/auth/me")).data
export const getHotelierApplication = async () => (await apiClient.get<HotelierApplication | null>("/api/auth/hotelier-application")).data
export const submitHotelierApplication = (request: BusinessApplicationRequest) => apiClient.post("/api/auth/hotelier-application", request)
