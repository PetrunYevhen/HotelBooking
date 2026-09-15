import { useState, type FormEvent } from "react"
import { useNavigate, useSearchParams } from "react-router-dom"
import { signIn, signUp, type BusinessApplicationRequest, type SignUpRequest } from "@/domains/auth/api/auth"
import { getAuthSession } from "@/domains/auth/session"
import { ApiError } from "@/shared/lib/api-client"

type Mode = "signIn" | "signUp"
type Intent = "guest" | "list-property"
type SignUpForm = Omit<SignUpRequest, "businessApplication"> & BusinessApplicationRequest & { confirmPassword: string }

const initialForm: SignUpForm = { firstName: "", lastName: "", username: "", email: "", phoneNumber: "", password: "", confirmPassword: "", intent: "guest", legalBusinessName: "", registrationNumber: "", taxNumber: "", businessEmail: "", businessPhoneNumber: "", firstPropertyName: "", firstPropertyAddress: "" }
const e164Phone = /^\+[1-9]\d{1,14}$/

export function SignIn() {
    const navigate = useNavigate()
    const [searchParams] = useSearchParams()
    const [mode, setMode] = useState<Mode>("signIn")
    const [usernameOrEmail, setUsernameOrEmail] = useState("")
    const [password, setPassword] = useState("")
    const [form, setForm] = useState(initialForm)
    const [error, setError] = useState("")
    const [saving, setSaving] = useState(false)
    const returnTo = searchParams.get("returnTo")
    const destination = () => returnTo || (getAuthSession()?.role === "Admin" ? "/admin" : getAuthSession()?.role === "Hotelier" ? "/hotelier" : "/account")
    const update = <K extends keyof SignUpForm>(key: K, value: SignUpForm[K]) => setForm(current => ({ ...current, [key]: value }))

    async function submitSignIn(event: FormEvent<HTMLFormElement>) {
        event.preventDefault(); setSaving(true); setError("")
        try { await signIn(usernameOrEmail, password); navigate(destination()) }
        catch (cause) { setError(cause instanceof ApiError ? cause.message : "Could not sign in.") }
        finally { setSaving(false) }
    }

    async function submitSignUp(event: FormEvent<HTMLFormElement>) {
        event.preventDefault()
        const base = [form.firstName, form.lastName, form.username, form.email, form.phoneNumber, form.password]
        const business = [form.legalBusinessName, form.registrationNumber, form.businessEmail, form.businessPhoneNumber, form.firstPropertyName, form.firstPropertyAddress]
        if (base.some(value => !value.trim()) || !form.confirmPassword || (form.intent === "list-property" && business.some(value => !value.trim()))) return setError("Complete all required fields.")
        if (!e164Phone.test(form.phoneNumber) || (form.intent === "list-property" && !e164Phone.test(form.businessPhoneNumber))) return setError("Phone numbers must use E.164 format (e.g. +380501234567).")
        if (form.password.length < 12) return setError("Password must be at least 12 characters.")
        if (form.password !== form.confirmPassword) return setError("Passwords do not match.")
        const request: SignUpRequest = { firstName: form.firstName.trim(), lastName: form.lastName.trim(), username: form.username.trim(), email: form.email.trim(), phoneNumber: form.phoneNumber.trim(), password: form.password, intent: form.intent,
            businessApplication: form.intent === "list-property" ? { legalBusinessName: form.legalBusinessName.trim(), registrationNumber: form.registrationNumber.trim(), taxNumber: form.taxNumber?.trim() || undefined, businessEmail: form.businessEmail.trim(), businessPhoneNumber: form.businessPhoneNumber.trim(), firstPropertyName: form.firstPropertyName.trim(), firstPropertyAddress: form.firstPropertyAddress.trim() } : undefined }
        setSaving(true); setError("")
        try { await signUp(request); navigate(destination()) }
        catch (cause) { setError(cause instanceof ApiError ? cause.message : "Could not create your account.") }
        finally { setSaving(false) }
    }

    return <main className="stayora-container py-16"><div className="mx-auto max-w-2xl rounded-xl border bg-white p-6 shadow-md"><p className="section-eyebrow">Account</p><div className="mt-4 grid grid-cols-2 rounded-lg bg-muted p-1"><Tab active={mode === "signIn"} onClick={() => { setMode("signIn"); setError("") }}>Sign in</Tab><Tab active={mode === "signUp"} onClick={() => { setMode("signUp"); setError("") }}>Create account</Tab></div>{mode === "signIn" ? <form onSubmit={submitSignIn}><h1 className="section-title mt-5">Sign in to continue</h1>{error && <Error text={error} />}<Field label="Email or username" value={usernameOrEmail} onChange={setUsernameOrEmail} /><Field label="Password" value={password} onChange={setPassword} type="password" /><Submit saving={saving} label="Sign in" /></form> : <form noValidate onSubmit={submitSignUp}><h1 className="section-title mt-5">Create your account</h1><p className="mt-2 text-sm text-text-secondary">Choose how you plan to use Stayora. Listing a property starts an approval request; it does not grant hotelier access immediately.</p>{error && <Error text={error} />}<div className="mt-5 grid gap-3 sm:grid-cols-2"><IntentCard intent="guest" active={form.intent === "guest"} onClick={() => update("intent", "guest")} title="Book stays" description="Create a guest account for bookings and saved trips." /><IntentCard intent="list-property" active={form.intent === "list-property"} onClick={() => update("intent", "list-property")} title="List a property" description="Create a guest account and submit a hotelier application." /></div><div className="mt-5 grid gap-4 sm:grid-cols-2"><Field label="First name" value={form.firstName} onChange={value => update("firstName", value)} /><Field label="Last name" value={form.lastName} onChange={value => update("lastName", value)} /></div><div className="grid gap-4 sm:grid-cols-2"><Field label="Username" value={form.username} onChange={value => update("username", value)} /><Field label="Email address" value={form.email} onChange={value => update("email", value)} type="email" /></div><div className="grid gap-4 sm:grid-cols-2"><Field label="Phone number" value={form.phoneNumber} onChange={value => update("phoneNumber", value)} placeholder="+380501234567" /><Field label="Password" value={form.password} onChange={value => update("password", value)} type="password" /></div><Field label="Confirm password" value={form.confirmPassword} onChange={value => update("confirmPassword", value)} type="password" />{form.intent === "list-property" && <section className="mt-6 border-t pt-5"><h2 className="font-heading text-xl font-semibold text-primary-900">Business and first property</h2><div className="mt-4 grid gap-4 sm:grid-cols-2"><Field label="Legal business name" value={form.legalBusinessName} onChange={value => update("legalBusinessName", value)} /><Field label="Registration number" value={form.registrationNumber} onChange={value => update("registrationNumber", value)} /><Field label="Tax number (optional)" value={form.taxNumber} onChange={value => update("taxNumber", value)} required={false} /><Field label="Business email" value={form.businessEmail} onChange={value => update("businessEmail", value)} type="email" /><Field label="Business phone" value={form.businessPhoneNumber} onChange={value => update("businessPhoneNumber", value)} placeholder="+380501234567" /><Field label="First property name" value={form.firstPropertyName} onChange={value => update("firstPropertyName", value)} /></div><Field label="First property address" value={form.firstPropertyAddress} onChange={value => update("firstPropertyAddress", value)} /></section>}<Submit saving={saving} label="Create account" /></form>}</div></main>
}

function Tab({ active, onClick, children }: { active: boolean; onClick: () => void; children: string }) { return <button type="button" onClick={onClick} className={`rounded-md px-3 py-2 text-sm font-semibold ${active ? "bg-white text-primary-900 shadow-sm" : "text-text-secondary"}`}>{children}</button> }
function IntentCard({ active, onClick, title, description }: { intent: Intent; active: boolean; onClick: () => void; title: string; description: string }) { return <button type="button" onClick={onClick} className={`rounded-lg border p-4 text-left ${active ? "border-primary-900 bg-primary-50" : "border-border hover:border-primary-400"}`}><span className="block font-semibold text-primary-900">{title}</span><span className="mt-1 block text-sm text-text-secondary">{description}</span></button> }
function Field({ label, value, onChange, type = "text", placeholder, required = true }: { label: string; value: string | undefined; onChange: (value: string) => void; type?: string; placeholder?: string; required?: boolean }) { return <label className="mt-4 block text-sm font-medium text-primary-900">{label}<input required={required} type={type} value={value ?? ""} onChange={event => onChange(event.target.value)} placeholder={placeholder} className="mt-1 w-full rounded-lg border px-3 py-2" /></label> }
function Submit({ saving, label }: { saving: boolean; label: string }) { return <button disabled={saving} className="mt-6 w-full rounded-lg bg-primary-900 px-4 py-2 text-sm font-semibold text-white disabled:opacity-50">{saving ? "Saving…" : label}</button> }
function Error({ text }: { text: string }) { return <p role="alert" className="mt-4 text-sm text-error-600">{text}</p> }
