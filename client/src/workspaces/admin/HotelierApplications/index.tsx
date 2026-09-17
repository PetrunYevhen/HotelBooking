import { useCallback, useEffect, useState } from "react"
import { approveApplication, getApplications, rejectApplication } from "@/domains/accommodations/api/admin"
import type { HotelierApplication } from "@/domains/auth/api/auth"
import { ApiError } from "@/shared/lib/api-client"
import { Button } from "@/shared/ui/button"
import { Badge } from "@/shared/ui/badge"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/shared/ui/select"
import { Table, TableBody, TableCell, TableEmpty, TableHead, TableHeader, TableRow } from "@/shared/ui/table"
import { RejectDialog } from "@/workspaces/admin/HotelierApplications/RejectDialog"

type StatusFilter = "Pending" | "Approved" | "Rejected" | "All"

const statusVariant: Record<HotelierApplication["status"], "warning" | "success" | "destructive"> = {
  Pending: "warning",
  Approved: "success",
  Rejected: "destructive",
}

export function HotelierApplications() {
  const [status, setStatus] = useState<StatusFilter>("Pending")
  const [applications, setApplications] = useState<HotelierApplication[]>([])
  const [error, setError] = useState("")
  const [loading, setLoading] = useState(true)
  const [rejecting, setRejecting] = useState<HotelierApplication | null>(null)

  const load = useCallback(async () => {
    setLoading(true)
    try {
      const data = await getApplications(status === "All" ? undefined : status)
      setApplications(data)
      setError("")
    } catch (cause) {
      setError(cause instanceof ApiError ? cause.message : "Could not load hotelier applications.")
    } finally {
      setLoading(false)
    }
  }, [status])

  useEffect(() => { void load() }, [load])

  async function approve(application: HotelierApplication) {
    try {
      await approveApplication(application.hotelierApplicationId)
      await load()
    } catch (cause) {
      setError(cause instanceof ApiError ? cause.message : "Could not approve application.")
    }
  }

  async function reject(reason: string) {
    if (!rejecting) return
    try {
      await rejectApplication(rejecting.hotelierApplicationId, reason)
      setRejecting(null)
      await load()
    } catch (cause) {
      setError(cause instanceof ApiError ? cause.message : "Could not reject application.")
    }
  }

  return (
    <section>
      <div className="flex flex-col justify-between gap-3 sm:flex-row sm:items-center">
        <h2 className="font-heading text-2xl font-semibold text-primary-900">Hotelier applications</h2>
        <Select value={status} onValueChange={value => setStatus(value as StatusFilter)}>
          <SelectTrigger className="w-40"><SelectValue /></SelectTrigger>
          <SelectContent>
            <SelectItem value="Pending">Pending</SelectItem>
            <SelectItem value="Approved">Approved</SelectItem>
            <SelectItem value="Rejected">Rejected</SelectItem>
            <SelectItem value="All">All</SelectItem>
          </SelectContent>
        </Select>
      </div>

      {error && <p role="alert" className="mt-4 rounded-lg bg-error-50 p-3 text-sm text-error-600">{error}</p>}

      <div className="mt-4">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Business</TableHead>
              <TableHead>Property</TableHead>
              <TableHead>Registration / contact</TableHead>
              <TableHead>Submitted</TableHead>
              <TableHead>Status</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {loading && <TableEmpty colSpan={6}>Loading…</TableEmpty>}
            {!loading && !applications.length && <TableEmpty colSpan={6}>No {status !== "All" ? status.toLowerCase() : ""} applications.</TableEmpty>}
            {!loading && applications.map(application => (
              <TableRow key={application.hotelierApplicationId}>
                <TableCell className="font-medium text-primary-900">{application.legalBusinessName}</TableCell>
                <TableCell>{application.firstPropertyName}<br /><span className="text-xs text-text-muted">{application.firstPropertyAddress}</span></TableCell>
                <TableCell>{application.registrationNumber}<br /><span className="text-xs text-text-muted">{application.businessEmail}</span></TableCell>
                <TableCell>{new Date(application.submittedAt).toLocaleDateString()}</TableCell>
                <TableCell><Badge variant={statusVariant[application.status]}>{application.status}</Badge></TableCell>
                <TableCell className="text-right">
                  {application.status === "Pending" && (
                    <div className="flex justify-end gap-2">
                      <Button size="sm" onClick={() => void approve(application)}>Approve</Button>
                      <Button size="sm" variant="outline" onClick={() => setRejecting(application)}>Reject</Button>
                    </div>
                  )}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>

      {rejecting && (
        <RejectDialog
          applicationName={rejecting.legalBusinessName}
          open={!!rejecting}
          onOpenChange={open => { if (!open) setRejecting(null) }}
          onConfirm={reject}
        />
      )}
    </section>
  )
}
