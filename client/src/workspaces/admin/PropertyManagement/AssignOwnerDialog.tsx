import { useState } from "react"
import type { HotelierHotel } from "@/domains/accommodations/api/hotelier"
import { Button } from "@/shared/ui/button"
import { Input } from "@/shared/ui/input"
import { Label } from "@/shared/ui/label"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/shared/ui/dialog"

export function AssignOwnerDialog({
  hotel,
  open,
  onOpenChange,
  onConfirm,
}: {
  hotel: HotelierHotel
  open: boolean
  onOpenChange: (open: boolean) => void
  onConfirm: (ownerUserId: string | null) => Promise<void>
}) {
  const [ownerUserId, setOwnerUserId] = useState(hotel.ownerUserId ?? "")
  const [submitting, setSubmitting] = useState(false)

  async function submit() {
    setSubmitting(true)
    try {
      await onConfirm(ownerUserId.trim() || null)
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <Dialog open={open} onOpenChange={next => { if (!submitting) onOpenChange(next) }}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Assign owner</DialogTitle>
          <DialogDescription>{hotel.name} — set the user who owns this property. Leave empty to unassign.</DialogDescription>
        </DialogHeader>
        <div className="mt-4">
          <Label htmlFor="owner-user-id">Owner user ID</Label>
          <Input
            id="owner-user-id"
            className="mt-1.5"
            value={ownerUserId}
            onChange={event => setOwnerUserId(event.target.value)}
            placeholder="User GUID"
          />
        </div>
        <DialogFooter>
          <Button variant="outline" size="sm" onClick={() => onOpenChange(false)} disabled={submitting}>
            Cancel
          </Button>
          <Button size="sm" onClick={() => void submit()} disabled={submitting}>
            {submitting ? "Saving…" : "Save owner"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
