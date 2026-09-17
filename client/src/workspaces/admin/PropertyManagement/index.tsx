import { useEffect, useState } from "react"
import { assignPropertyOwner, getProperties } from "@/domains/accommodations/api/admin"
import type { HotelierHotel } from "@/domains/accommodations/api/hotelier"
import { ApiError } from "@/shared/lib/api-client"
import { Button } from "@/shared/ui/button"
import { Badge } from "@/shared/ui/badge"
import { Table, TableBody, TableCell, TableEmpty, TableHead, TableHeader, TableRow } from "@/shared/ui/table"
import { AssignOwnerDialog } from "@/workspaces/admin/PropertyManagement/AssignOwnerDialog"

export function PropertyManagement() {
  const [properties, setProperties] = useState<HotelierHotel[]>([])
  const [error, setError] = useState("")
  const [loading, setLoading] = useState(true)
  const [editing, setEditing] = useState<HotelierHotel | null>(null)

  async function load() {
    setLoading(true)
    try {
      setProperties(await getProperties())
      setError("")
    } catch (cause) {
      setError(cause instanceof ApiError ? cause.message : "Could not load properties.")
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { void load() }, [])

  async function saveOwner(ownerUserId: string | null) {
    if (!editing) return
    try {
      await assignPropertyOwner(editing.hotelId, ownerUserId)
      setEditing(null)
      await load()
    } catch (cause) {
      setError(cause instanceof ApiError ? cause.message : "Owner could not be assigned.")
    }
  }

  return (
    <section>
      <h2 className="font-heading text-2xl font-semibold text-primary-900">Property management</h2>

      {error && <p role="alert" className="mt-4 rounded-lg bg-error-50 p-3 text-sm text-error-600">{error}</p>}

      <div className="mt-4">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Name</TableHead>
              <TableHead>Location</TableHead>
              <TableHead>Owner</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {loading && <TableEmpty colSpan={4}>Loading…</TableEmpty>}
            {!loading && !properties.length && <TableEmpty colSpan={4}>No properties yet.</TableEmpty>}
            {!loading && properties.map(hotel => (
              <TableRow key={hotel.hotelId}>
                <TableCell className="font-medium text-primary-900">{hotel.name}</TableCell>
                <TableCell>{hotel.city}, {hotel.country}</TableCell>
                <TableCell>
                  {hotel.ownerUserId
                    ? <span className="font-mono text-xs text-text-secondary">{hotel.ownerUserId}</span>
                    : <Badge variant="warning">Unassigned</Badge>}
                </TableCell>
                <TableCell className="text-right">
                  <Button size="sm" variant="outline" onClick={() => setEditing(hotel)}>Assign owner</Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>

      {editing && (
        <AssignOwnerDialog
          hotel={editing}
          open={!!editing}
          onOpenChange={open => { if (!open) setEditing(null) }}
          onConfirm={saveOwner}
        />
      )}
    </section>
  )
}
