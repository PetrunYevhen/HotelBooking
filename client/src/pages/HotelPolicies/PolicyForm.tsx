import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { cancellationTypes, petPolicies, smokingPolicies } from "./constants"

export interface PolicyFormValues {
    cancellationPolicyType: string
    deadlineDays: number | ""
    percentagePenalty: number | ""
    petPolicy: string
    smokingPolicy: string
    checkOutHoursPolicy: number
}

export function PolicyForm({ values, saving, onChange, onSubmit }: { values: PolicyFormValues; saving: boolean; onChange: (values: PolicyFormValues) => void; onSubmit: (event: React.FormEvent) => void }) {
    const needsDeadline = values.cancellationPolicyType !== "NonRefundable"
    const needsPenalty = values.cancellationPolicyType === "PartialRefund"
    const set = <Key extends keyof PolicyFormValues>(key: Key, value: PolicyFormValues[Key]) => onChange({ ...values, [key]: value })
    return <form onSubmit={onSubmit} className="flex max-w-md flex-col gap-6"><h1 className="text-xl font-semibold">Hotel Policies</h1><PolicySelect label="Cancellation Policy" value={values.cancellationPolicyType} items={cancellationTypes} onChange={(value) => set("cancellationPolicyType", value)} />{needsDeadline && <NumberField label="Deadline (days before check-in)" value={values.deadlineDays} onChange={(value) => set("deadlineDays", value)} />}{needsPenalty && <NumberField label="Penalty (%)" value={values.percentagePenalty} onChange={(value) => set("percentagePenalty", value)} />}<PolicySelect label="Pet Policy" value={values.petPolicy} items={petPolicies} onChange={(value) => set("petPolicy", value)} /><PolicySelect label="Smoking Policy" value={values.smokingPolicy} items={smokingPolicies} onChange={(value) => set("smokingPolicy", value)} /><NumberField label="Check-out Hour" value={values.checkOutHoursPolicy} min={0} max={23} onChange={(value) => set("checkOutHoursPolicy", Number(value))} /><Button type="submit" disabled={saving}>{saving ? "Saving..." : "Save Changes"}</Button></form>
}

function PolicySelect({ label, value, items, onChange }: { label: string; value: string; items: string[]; onChange: (value: string) => void }) {
    return <div className="flex flex-col gap-2"><Label>{label}</Label><Select value={value} onValueChange={(next) => next && onChange(next)}><SelectTrigger><SelectValue /></SelectTrigger><SelectContent>{items.map((item) => <SelectItem key={item} value={item}>{item}</SelectItem>)}</SelectContent></Select></div>
}

function NumberField({ label, value, min, max, onChange }: { label: string; value: number | ""; min?: number; max?: number; onChange: (value: number | "") => void }) {
    return <div className="flex flex-col gap-2"><Label>{label}</Label><Input type="number" min={min} max={max} value={value} onChange={(event) => onChange(event.target.value === "" ? "" : Number(event.target.value))} /></div>
}
