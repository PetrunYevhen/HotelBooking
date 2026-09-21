import { useState } from "react"
import { createRoom } from "@/domains/accommodations/api/hotelier"
import { ApiError } from "@/shared/lib/api-client"
import { Button } from "@/shared/ui/button"
import { Input } from "@/shared/ui/input"
import { Label } from "@/shared/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/shared/ui/select"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/shared/ui/dialog"

const roomTypes = [
  { value: 1, label: "Single" },
  { value: 2, label: "Double" },
  { value: 3, label: "Triple" },
  { value: 4, label: "Suite" },
  { value: 5, label: "Deluxe" },
  { value: 6, label: "Penthouse" },
  { value: 7, label: "Presidential" },
]

interface FormValues {
  roomNumber: string
  type: number
  beds: number | ""
  capacity: number | ""
  description: string
  basePriceAmount: number | ""
}

const initialValues: FormValues = { roomNumber: "", type: 1, beds: "", capacity: "", description: "", basePriceAmount: "" }

export function AddRoomDialog({
  hotelId,
  open,
  onOpenChange,
  onCreated,
}: {
  hotelId: string
  open: boolean
  onOpenChange: (open: boolean) => void
  onCreated: () => Promise<void>
}) {
  const [values, setValues] = useState(initialValues)
  const [error, setError] = useState("")
  const [submitting, setSubmitting] = useState(false)

  const set = <Key extends keyof FormValues>(key: Key, value: FormValues[Key]) => setValues(current => ({ ...current, [key]: value }))
  const valid = values.roomNumber.trim() && values.beds !== "" && values.capacity !== "" && values.basePriceAmount !== ""

  async function submit() {
    if (!valid) return
    setSubmitting(true)
    setError("")
    try {
      await createRoom({
        hotelId,
        roomNumber: values.roomNumber.trim(),
        type: values.type,
        beds: Number(values.beds),
        capacity: Number(values.capacity),
        description: values.description.trim() || null,
        status: 1,
        basePriceAmount: Number(values.basePriceAmount),
        basePriceCurrency: "EUR",
      })
      setValues(initialValues)
      onOpenChange(false)
      await onCreated()
    } catch (cause) {
      setError(cause instanceof ApiError ? cause.message : "Could not create the room.")
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <Dialog open={open} onOpenChange={next => { if (!submitting) { onOpenChange(next); if (!next) { setValues(initialValues); setError("") } } }}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Add room</DialogTitle>
          <DialogDescription>Set up a new room for this property.</DialogDescription>
        </DialogHeader>

        {error && <p role="alert" className="mt-2 rounded-lg bg-error-50 p-3 text-sm text-error-600">{error}</p>}

        <div className="mt-4 grid gap-4 sm:grid-cols-2">
          <Field label="Room number"><Input value={values.roomNumber} onChange={event => set("roomNumber", event.target.value)} /></Field>
          <Field label="Type">
            <Select value={String(values.type)} onValueChange={next => next && set("type", Number(next))}>
              <SelectTrigger className="w-full"><SelectValue>{(value: string) => roomTypes.find(item => String(item.value) === value)?.label}</SelectValue></SelectTrigger>
              <SelectContent>
                {roomTypes.map(item => <SelectItem key={item.value} value={String(item.value)}>{item.label}</SelectItem>)}
              </SelectContent>
            </Select>
          </Field>
          <Field label="Beds"><Input type="number" min={1} value={values.beds} onChange={event => set("beds", event.target.value === "" ? "" : Number(event.target.value))} /></Field>
          <Field label="Capacity"><Input type="number" min={1} value={values.capacity} onChange={event => set("capacity", event.target.value === "" ? "" : Number(event.target.value))} /></Field>
          <Field label="Base price (EUR)"><Input type="number" min={0} step="0.01" value={values.basePriceAmount} onChange={event => set("basePriceAmount", event.target.value === "" ? "" : Number(event.target.value))} /></Field>
          <div className="sm:col-span-2">
            <Field label="Description (optional)"><Input value={values.description} onChange={event => set("description", event.target.value)} /></Field>
          </div>
        </div>

        <DialogFooter>
          <Button variant="outline" size="sm" onClick={() => onOpenChange(false)} disabled={submitting}>Cancel</Button>
          <Button size="sm" onClick={() => void submit()} disabled={!valid || submitting}>{submitting ? "Adding…" : "Add room"}</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}

function Field({ label, children }: { label: string; children: React.ReactNode }) {
  return <div className="flex flex-col gap-1.5"><Label>{label}</Label>{children}</div>
}
