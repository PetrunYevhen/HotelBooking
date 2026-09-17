import { useState } from "react"
import { Button } from "@/shared/ui/button"
import { Label } from "@/shared/ui/label"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/shared/ui/dialog"

export function RejectDialog({
  applicationName,
  open,
  onOpenChange,
  onConfirm,
}: {
  applicationName: string
  open: boolean
  onOpenChange: (open: boolean) => void
  onConfirm: (reason: string) => Promise<void>
}) {
  const [reason, setReason] = useState("")
  const [submitting, setSubmitting] = useState(false)

  async function submit() {
    const trimmed = reason.trim()
    if (!trimmed) return
    setSubmitting(true)
    try {
      await onConfirm(trimmed)
      setReason("")
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <Dialog open={open} onOpenChange={next => { if (!submitting) { onOpenChange(next); if (!next) setReason("") } }}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Reject application</DialogTitle>
          <DialogDescription>{applicationName} — explain why this hotelier application is being rejected.</DialogDescription>
        </DialogHeader>
        <div className="mt-4">
          <Label htmlFor="reject-reason">Reason</Label>
          <textarea
            id="reject-reason"
            value={reason}
            onChange={event => setReason(event.target.value)}
            rows={4}
            className="mt-1.5 w-full rounded-md border border-input bg-white px-3.5 py-2 text-sm text-foreground outline-none focus-visible:border-ring focus-visible:ring-3 focus-visible:ring-ring/15"
            placeholder="e.g. Registration number could not be verified."
          />
        </div>
        <DialogFooter>
          <Button variant="outline" size="sm" onClick={() => onOpenChange(false)} disabled={submitting}>
            Cancel
          </Button>
          <Button variant="destructive" size="sm" onClick={() => void submit()} disabled={!reason.trim() || submitting}>
            {submitting ? "Rejecting…" : "Reject application"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
