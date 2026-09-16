import axios from "axios"

export interface ApiProblem {
    status?: number
    title?: string
    detail?: string
    code?: string
}

export class ApiError extends Error {
    readonly problem?: ApiProblem

    constructor(message: string, problem?: ApiProblem) {
        super(message)
        this.name = "ApiError"
        this.problem = problem
    }
}

export const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL,
    withCredentials: true,
    headers: { "Content-Type": "application/json" },
})

// In-memory only - never persisted (localStorage/sessionStorage are readable by any
// script on the page, so a stolen access token there survives an XSS hit indefinitely).
// Session persistence across reloads comes from the httpOnly refresh cookie instead;
// see restoreSession() in domains/auth/api/auth.ts.
let accessToken: string | null = null

export const hasAccessToken = () => accessToken !== null
export const getAccessToken = () => accessToken
export const setAccessToken = (token: string) => { accessToken = token }
export const clearAccessToken = () => { accessToken = null }

const csrfCookieName = "hotelbooking_csrf"

export const readCsrfCookie = (): string | null => {
    const match = document.cookie.match(new RegExp(`(?:^|; )${csrfCookieName}=([^;]*)`))
    return match ? decodeURIComponent(match[1]) : null
}

apiClient.interceptors.request.use((config) => {
    if (accessToken) config.headers.Authorization = `Bearer ${accessToken}`
    return config
})

apiClient.interceptors.response.use(
    (response) => response,
    (error: unknown) => {
        if (axios.isAxiosError<ApiProblem>(error)) {
            const problem = error.response?.data
            return Promise.reject(new ApiError(problem?.detail ?? "The request failed.", problem))
        }

        return Promise.reject(error)
    },
)
