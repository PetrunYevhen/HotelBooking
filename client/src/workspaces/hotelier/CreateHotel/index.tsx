import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { createHotel } from "@/domains/accommodations/api/hotelier"
import { ApiError } from "@/shared/lib/api-client"
import { Button } from "@/shared/ui/button"
import { Input } from "@/shared/ui/input"
import { Label } from "@/shared/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/shared/ui/select"

const statuses = [
  { value: 1, label: "Active" },
  { value: 2, label: "Inactive" },
  { value: 3, label: "Under renovation" },
  { value: 4, label: "Permanently closed" },
]

interface FormValues {
  name: string
  description: string
  status: number
  street: string
  city: string
  country: string
  postalCode: string
  checkIn: string
  checkOut: string
}

const initialValues: FormValues = {
  name: "",
  description: "",
  status: 1,
  street: "",
  city: "",
  country: "",
  postalCode: "",
  checkIn: "14:00",
  checkOut: "11:00",
}

export function CreateHotel() {
  const navigate = useNavigate()
  const [values, setValues] = useState(initialValues)
  const [error, setError] = useState("")
  const [saving, setSaving] = useState(false)

  const set = <Key extends keyof FormValues>(key: Key, value: FormValues[Key]) => setValues(current => ({ ...current, [key]: value }))

  async function handleSubmit(event: React.FormEvent) {
    event.preventDefault()
    setSaving(true)
    setError("")
    try {
      const { id } = await createHotel({
        name: values.name,
        description: values.description,
        status: values.status,
        street: values.street,
        city: values.city,
        country: values.country,
        postalCode: values.postalCode,
        checkIn: `${values.checkIn}:00`,
        checkOut: `${values.checkOut}:00`,
      })
      navigate(`/hotelier/hotels/${id}/overview`)
    } catch (cause) {
      setError(cause instanceof ApiError ? cause.message : "Could not create the hotel.")
      setSaving(false)
    }
  }

  return (
    <main className="stayora-container stayora-page">
      <p className="section-eyebrow">Hotelier workspace</p>
      <h1 className="section-title mt-1">Add a property</h1>

      {error && <p role="alert" className="mt-4 rounded-lg bg-error-50 p-3 text-sm text-error-600">{error}</p>}

      <form onSubmit={handleSubmit} className="mt-6 flex max-w-xl flex-col gap-5">
        <Field label="Hotel name">
          <Input value={values.name} onChange={event => set("name", event.target.value)} required />
        </Field>
        <Field label="Description">
          <textarea
            value={values.description}
            onChange={event => set("description", event.target.value)}
            rows={3}
            required
            className="w-full rounded-md border border-input bg-white px-3.5 py-2 text-sm text-foreground outline-none focus-visible:border-ring focus-visible:ring-3 focus-visible:ring-ring/15"
          />
        </Field>
        <div className="grid gap-5 sm:grid-cols-2">
          <Field label="Street">
            <Input value={values.street} onChange={event => set("street", event.target.value)} required />
          </Field>
          <Field label="City">
            <Input value={values.city} onChange={event => set("city", event.target.value)} required />
          </Field>
          <Field label="Country">
            <Input value={values.country} onChange={event => set("country", event.target.value)} required />
          </Field>
          <Field label="Postal code">
            <Input value={values.postalCode} onChange={event => set("postalCode", event.target.value)} required />
          </Field>
          <Field label="Check-in time">
            <Input type="time" value={values.checkIn} onChange={event => set("checkIn", event.target.value)} required />
          </Field>
          <Field label="Check-out time">
            <Input type="time" value={values.checkOut} onChange={event => set("checkOut", event.target.value)} required />
          </Field>
        </div>
        <Field label="Status">
          <Select value={String(values.status)} onValueChange={next => next && set("status", Number(next))}>
            <SelectTrigger className="w-full"><SelectValue>{(value: string) => statuses.find(item => String(item.value) === value)?.label}</SelectValue></SelectTrigger>
            <SelectContent>
              {statuses.map(item => <SelectItem key={item.value} value={String(item.value)}>{item.label}</SelectItem>)}
            </SelectContent>
          </Select>
        </Field>
        <Button type="submit" disabled={saving}>{saving ? "Creating…" : "Create hotel"}</Button>
      </form>
    </main>
  )
}

function Field({ label, children }: { label: string; children: React.ReactNode }) {
  return <div className="flex flex-col gap-2"><Label>{label}</Label>{children}</div>
}
