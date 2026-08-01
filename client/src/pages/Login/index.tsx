import { useState, type FormEvent } from "react"
import { useNavigate, useSearchParams } from "react-router-dom"
import { login } from "@/api/auth"
import { ApiError } from "@/lib/api-client"

export function Login() {
    const navigate = useNavigate(); const [searchParams] = useSearchParams()
    const [usernameOrEmail, setUsernameOrEmail] = useState(""); const [password, setPassword] = useState(""); const [error, setError] = useState(""); const [saving, setSaving] = useState(false)
    async function submit(event: FormEvent<HTMLFormElement>) { event.preventDefault(); setSaving(true); setError(""); try { await login(usernameOrEmail, password); navigate(searchParams.get("returnTo") || "/") } catch (cause) { setError(cause instanceof ApiError ? cause.message : "Could not sign in.") } finally { setSaving(false) } }
    return <main className="stayora-container py-16"><form onSubmit={submit} className="mx-auto max-w-md rounded-xl border bg-white p-6 shadow-md"><p className="section-eyebrow">Account</p><h1 className="section-title mt-1">Sign in to continue</h1><p className="mt-2 text-sm text-text-secondary">Bookings are linked to your account.</p>{error && <p role="alert" className="mt-4 text-sm text-error-600">{error}</p>}<label className="mt-5 block text-sm font-medium text-primary-900">Email or username<input required value={usernameOrEmail} onChange={event => setUsernameOrEmail(event.target.value)} className="mt-1 w-full rounded-lg border px-3 py-2" /></label><label className="mt-4 block text-sm font-medium text-primary-900">Password<input required type="password" value={password} onChange={event => setPassword(event.target.value)} className="mt-1 w-full rounded-lg border px-3 py-2" /></label><button disabled={saving} className="mt-6 w-full rounded-lg bg-primary-900 px-4 py-2 text-sm font-semibold text-white disabled:opacity-50">{saving ? "Signing in…" : "Sign in"}</button></form></main>
}
