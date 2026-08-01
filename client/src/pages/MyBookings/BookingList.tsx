import { ArrowRight, CalendarDays, CircleDollarSign } from "lucide-react"
import { Link } from "react-router-dom"
import type { BookingDto } from "@/api/bookings"
import { cancellableStatuses, statusStyles } from "./constants"

interface BookingListProps {
    bookings: BookingDto[]
    hotelNames: Record<string, string>
    cancellingId: string | null
    onCancel: (event: React.MouseEvent, bookingId: string) => void
}

export function BookingList({ bookings, hotelNames, cancellingId, onCancel }: BookingListProps) {
    return (
        <div className="grid gap-4">
            {bookings.map((booking) => {
                const isCancellable = cancellableStatuses.has(booking.status)

                return (
                    <article key={booking.id} className="overflow-hidden rounded-xl border bg-white shadow-sm transition-shadow hover:shadow-md">
                        <Link to={`/booking-confirmation/${booking.id}`} className="group grid gap-5 p-5 sm:grid-cols-[auto_minmax(0,1fr)_auto] sm:items-center sm:p-6">
                            <span className="flex size-12 shrink-0 items-center justify-center rounded-full bg-gold-100 text-gold-600">
                                <CalendarDays size={22} />
                            </span>

                            <div className="min-w-0">
                                <div className="flex flex-wrap items-center gap-x-3 gap-y-2">
                                    <h2 className="truncate font-heading text-xl font-semibold text-primary-900 group-hover:text-primary-700">
                                        {hotelNames[booking.hotelId] ?? "Loading hotel…"}
                                    </h2>
                                    <span className={`inline-flex min-h-6 items-center rounded-full px-2.5 py-1 text-xs font-semibold ${statusStyles[booking.status] ?? "bg-muted text-text-secondary"}`}>
                                        {booking.status}
                                    </span>
                                </div>
                                <p className="mt-2 text-sm font-medium text-text-secondary">
                                    {formatStayDates(booking.checkInDate, booking.checkOutDate)}
                                </p>
                                <p className="mt-1 text-xs text-text-muted">Reference: {booking.id}</p>
                                {booking.addOns && booking.addOns.length > 0 && (
                                    <p className="mt-3 text-sm text-text-secondary">
                                        Extras: {booking.addOns.map((addOn) => addOn.name).join(", ")}
                                    </p>
                                )}
                            </div>

                            <div className="flex items-center justify-between gap-4 border-t pt-4 sm:block sm:border-0 sm:pt-0 sm:text-right">
                                <div>
                                    <p className="text-xs text-text-muted">Total</p>
                                    <p className="mt-1 flex items-center gap-1 font-heading text-xl font-semibold text-primary-900 sm:justify-end">
                                        <CircleDollarSign size={17} className="text-gold-600" />
                                        {formatPrice(booking.totalPrice, booking.currency)}
                                    </p>
                                </div>
                                <span className="inline-flex items-center gap-1 text-sm font-semibold text-primary-700 group-hover:text-gold-600 sm:mt-3">
                                    View <ArrowRight size={16} />
                                </span>
                            </div>
                        </Link>

                        {isCancellable && (
                            <div className="border-t bg-bg-warm px-5 py-3 sm:px-6">
                                <button
                                    type="button"
                                    onClick={(event) => onCancel(event, booking.id)}
                                    disabled={cancellingId === booking.id}
                                    className="text-sm font-semibold text-error-600 transition-colors hover:text-error-600 hover:underline disabled:cursor-not-allowed disabled:opacity-50"
                                >
                                    {cancellingId === booking.id ? "Cancelling booking…" : "Cancel booking"}
                                </button>
                            </div>
                        )}
                    </article>
                )
            })}
        </div>
    )
}

function formatStayDates(checkIn: string, checkOut: string) {
    const formatter = new Intl.DateTimeFormat(undefined, { day: "numeric", month: "short", year: "numeric" })
    return `${formatter.format(new Date(checkIn))} — ${formatter.format(new Date(checkOut))}`
}

function formatPrice(amount: number, currency: string) {
    return new Intl.NumberFormat(undefined, { style: "currency", currency }).format(amount)
}
