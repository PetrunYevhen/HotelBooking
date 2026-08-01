import { apiClient, setAccessToken } from "@/lib/api-client"

interface AuthResponse { accessToken: string }

export async function login(usernameOrEmail: string, password: string): Promise<void> {
    const { data } = await apiClient.post<AuthResponse>("/api/auth/login", { usernameOrEmail, password })
    setAccessToken(data.accessToken)
}
