import { useState } from "react"
import { useParams } from "react-router-dom"
import { type SetHotelPoliciesRequest } from "@/api/hotels"
import { savePolicies } from "@/api/hotelier"
import { PolicyForm, type PolicyFormValues } from "./PolicyForm"

const initialValues: PolicyFormValues = { cancellationPolicyType: "NonRefundable", deadlineDays: "", percentagePenalty: "", petPolicy: "NotAllowed", smokingPolicy: "NonSmoking", checkOutHoursPolicy: 12 }

export function HotelPolicies() {
    const { id: hotelId } = useParams<{ id: string }>()
    const [values, setValues] = useState(initialValues)
    const [saving, setSaving] = useState(false)
    async function handleSubmit(event: React.FormEvent) {
        event.preventDefault()
        if (!hotelId) return
        const payload: SetHotelPoliciesRequest = { cancellationPolicyType: values.cancellationPolicyType, deadlineDays: values.cancellationPolicyType === "NonRefundable" ? null : Number(values.deadlineDays), percentagePenalty: values.cancellationPolicyType === "PartialRefund" ? Number(values.percentagePenalty) : null, petPolicy: values.petPolicy, smokingPolicy: values.smokingPolicy, checkOutHoursPolicy: values.checkOutHoursPolicy }
        setSaving(true)
        try { await savePolicies(hotelId, payload) } finally { setSaving(false) }
    }
    return <PolicyForm values={values} saving={saving} onChange={setValues} onSubmit={handleSubmit} />
}
