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
    headers: { "Content-Type": "application/json" },
})

const accessTokenKey = "hotelbooking_access_token"

export const hasAccessToken = () => Boolean(localStorage.getItem(accessTokenKey))
export const setAccessToken = (token: string) => localStorage.setItem(accessTokenKey, token)
export const clearAccessToken = () => localStorage.removeItem(accessTokenKey)

apiClient.interceptors.request.use((config) => {
    const token = localStorage.getItem(accessTokenKey)
    if (token) config.headers.Authorization = `Bearer ${token}`
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
