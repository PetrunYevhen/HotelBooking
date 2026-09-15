import { clearAccessToken, getAccessToken } from "@/shared/lib/api-client"

export type WorkspaceRole = "Guest" | "Hotelier" | "Admin"

interface JwtPayload { sub?: string; unique_name?: string; role?: string; exp?: number }

export interface AuthSession { userId: string; username: string; role: WorkspaceRole }

function readPayload(token: string): JwtPayload | null {
    try {
        const encoded = token.split(".")[1]
        if (!encoded) return null
        const normalized = encoded.replace(/-/g, "+").replace(/_/g, "/")
        return JSON.parse(atob(normalized.padEnd(normalized.length + (4 - normalized.length % 4) % 4, "="))) as JwtPayload
    } catch { return null }
}

export function getAuthSession(): AuthSession | null {
    const token = getAccessToken()
    if (!token) return null
    const payload = readPayload(token)
    if (!payload?.sub || !payload.exp || payload.exp * 1000 <= Date.now()) {
        clearAccessToken()
        return null
    }
    const role = payload.role === "Admin" ? "Admin" : payload.role === "Hotelier" ? "Hotelier" : "Guest"
    return { userId: payload.sub, username: payload.unique_name ?? "", role }
}

export const hasWorkspaceRole = (roles: WorkspaceRole[]) => {
    const session = getAuthSession()
    return session !== null && roles.includes(session.role)
}
