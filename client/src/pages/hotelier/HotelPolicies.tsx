import { useParams } from "react-router-dom"
import { useState } from "react"
import { setHotelPolicies, type SetHotelPoliciesRequest } from "@/api/hotels"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"

const cancellationTypes = ["FreeCancellation", "PartialRefund", "NonRefundable"]
const petPolicies = ["NotAllowed", "Allowed", "AllowedWithFee"]
const smokingPolicies = ["NonSmoking", "SmokingAllowed", "DesignatedAreas"]

export function HotelPolicies() {
  const { id: hotelId } = useParams<{ id: string }>()

  const [cancellationPolicyType, setCancellationPolicyType] = useState("NonRefundable")
  const [deadlineDays, setDeadlineDays] = useState<number | "">("")
  const [percentagePenalty, setPercentagePenalty] = useState<number | "">("")
  const [petPolicy, setPetPolicy] = useState("NotAllowed")
  const [smokingPolicy, setSmokingPolicy] = useState("NonSmoking")
  const [checkOutHoursPolicy, setCheckOutHoursPolicy] = useState(12)
  const [saving, setSaving] = useState(false)

  const needsDeadline = cancellationPolicyType !== "NonRefundable"
  const needsPenalty = cancellationPolicyType === "PartialRefund"

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!hotelId) return

    const payload: SetHotelPoliciesRequest = {
      cancellationPolicyType,
      deadlineDays: needsDeadline ? Number(deadlineDays) : null,
      percentagePenalty: needsPenalty ? Number(percentagePenalty) : null,
      petPolicy,
      smokingPolicy,
      checkOutHoursPolicy,
    }

    setSaving(true)
    try {
      await setHotelPolicies(hotelId, payload)
    } finally {
      setSaving(false)
    }
  }

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-6 max-w-md">
      <h1 className="text-xl font-semibold">Hotel Policies</h1>

      <div className="flex flex-col gap-2">
        <Label>Cancellation Policy</Label>
        <Select value={cancellationPolicyType} onValueChange={(value) => value && setCancellationPolicyType(value)}>
          <SelectTrigger>
            <SelectValue />
          </SelectTrigger>
          <SelectContent>
            {cancellationTypes.map((t) => (
              <SelectItem key={t} value={t}>{t}</SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      {needsDeadline && (
        <div className="flex flex-col gap-2">
          <Label>Deadline (days before check-in)</Label>
          <Input
            type="number"
            value={deadlineDays}
            onChange={(e) => setDeadlineDays(e.target.value === "" ? "" : Number(e.target.value))}
          />
        </div>
      )}

      {needsPenalty && (
        <div className="flex flex-col gap-2">
          <Label>Penalty (%)</Label>
          <Input
            type="number"
            value={percentagePenalty}
            onChange={(e) => setPercentagePenalty(e.target.value === "" ? "" : Number(e.target.value))}
          />
        </div>
      )}

      <div className="flex flex-col gap-2">
        <Label>Pet Policy</Label>
        <Select value={petPolicy} onValueChange={(value) => value && setPetPolicy(value)}>
          <SelectTrigger>
            <SelectValue />
          </SelectTrigger>
          <SelectContent>
            {petPolicies.map((p) => (
              <SelectItem key={p} value={p}>{p}</SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      <div className="flex flex-col gap-2">
        <Label>Smoking Policy</Label>
        <Select value={smokingPolicy} onValueChange={(value) => value && setSmokingPolicy(value)}>
          <SelectTrigger>
            <SelectValue />
          </SelectTrigger>
          <SelectContent>
            {smokingPolicies.map((s) => (
              <SelectItem key={s} value={s}>{s}</SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      <div className="flex flex-col gap-2">
        <Label>Check-out Hour</Label>
        <Input
          type="number"
          min={0}
          max={23}
          value={checkOutHoursPolicy}
          onChange={(e) => setCheckOutHoursPolicy(Number(e.target.value))}
        />
      </div>

      <Button type="submit" disabled={saving}>
        {saving ? "Saving..." : "Save Changes"}
      </Button>
    </form>
  )
}
