import type { ReactNode } from "react"
import { Link } from "react-router-dom"
import { hasWorkspaceRole, type WorkspaceRole } from "@/domains/auth/session"

export function WorkspaceRoute({ roles, children }: { roles: WorkspaceRole[]; children: ReactNode }) {
    if (hasWorkspaceRole(roles)) return <>{children}</>
    return <main className="stayora-container stayora-page"><p className="section-eyebrow">Access denied</p><h1 className="section-title mt-1">This workspace is not available to your account.</h1><p className="mt-3 text-text-secondary">Sign in with an account that has the required access.</p><Link to="/signin" className="mt-5 inline-block rounded-lg bg-primary-900 px-4 py-2 text-sm font-semibold text-white">Sign in</Link></main>
}
