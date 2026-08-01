import { useCallback, useEffect, useState } from "react"
import { CalendarDays, LoaderCircle, SearchX } from "lucide-react"
import { Link } from "react-router-dom"
import { cancelBooking, getMyBookings, type BookingDto } from "@/api/bookings"
import { getHotelById } from "@/api/hotels"
import { ApiError } from "@/lib/api-client"
import { BookingList } from "./BookingList"

export function MyBookings() {
    const [bookings, setBookings] = useState<BookingDto[]>([])
    const [hotelNames, setHotelNames] = useState<Record<string, string>>({})
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [cancellingId, setCancellingId] = useState<string | null>(null)

    const fetchBookings = useCallback(() => {
        setError(null)

        return getMyBookings()
            .then(setBookings)
            .catch((reason: unknown) => {
                setError(reason instanceof ApiError ? reason.message : "Could not load bookings.")
            })
    }, [])

    useEffect(() => {
        fetchBookings().finally(() => setIsLoading(false))
    }, [fetchBookings])

    useEffect(() => {
        const ids = [...new Set(bookings.map((booking) => booking.hotelId))]
            .filter((id) => !(id in hotelNames))

        if (!ids.length) return

        void Promise.all(ids.map(async (id) => {
            try {
                const hotel = await getHotelById(id)
                return [id, hotel.name] as const
            } catch {
                return [id, "Hotel unavailable"] as const
            }
        })).then((names) => {
            setHotelNames((previous) => ({ ...previous, ...Object.fromEntries(names) }))
        })
    }, [bookings, hotelNames])

    async function handleCancel(event: React.MouseEvent, bookingId: string) {
        event.preventDefault()
        setCancellingId(bookingId)

        try {
            await cancelBooking(bookingId)
            await fetchBookings()
        } catch (reason: unknown) {
            setError(reason instanceof ApiError ? reason.message : "Could not cancel booking.")
        } finally {
            setCancellingId(null)
        }
    }

    return (
        <div className="stayora-page">
            <div className="stayora-container">
                <header className="flex flex-col justify-between gap-5 border-b border-border pb-7 sm:flex-row sm:items-end">
                    <div>
                        <p className="section-eyebrow">Your trips</p>
                        <h1 className="mt-1 font-heading text-4xl font-semibold leading-[1.2] text-primary-900 md:text-5xl">
                            My bookings
                        </h1>
                        <p className="mt-3 max-w-xl text-text-secondary">
                            Review your upcoming stays and manage existing reservations.
                        </p>
                    </div>

                    {!isLoading && !error && (
                        <span className="inline-flex w-fit items-center gap-2 rounded-full bg-gold-100 px-3 py-1.5 text-sm font-medium text-primary-800">
                            <CalendarDays size={16} className="text-gold-600" />
                            {bookings.length} {bookings.length === 1 ? "booking" : "bookings"}
                        </span>
                    )}
                </header>

                <section className="mt-8" aria-live="polite">
                    {isLoading && <LoadingState />}
                    {!isLoading && error && <ErrorState message={error} onRetry={() => void fetchBookings()} />}
                    {!isLoading && !error && !bookings.length && <EmptyState />}
                    {!isLoading && !error && bookings.length > 0 && (
                        <BookingList
                            bookings={bookings}
                            hotelNames={hotelNames}
                            cancellingId={cancellingId}
                            onCancel={handleCancel}
                        />
                    )}
                </section>
            </div>
        </div>
    )
}

function LoadingState() {
    return (
        <div className="flex min-h-72 flex-col items-center justify-center rounded-xl border border-dashed bg-white px-6 text-center">
            <LoaderCircle size={28} className="animate-spin text-gold-600" />
            <p className="mt-4 font-medium text-primary-900">Loading your bookings</p>
            <p className="mt-1 text-sm text-text-secondary">This will only take a moment.</p>
        </div>
    )
}

function ErrorState({ message, onRetry }: { message: string; onRetry: () => void }) {
    return (
        <div role="alert" className="flex min-h-72 flex-col items-center justify-center rounded-xl border border-error-100 bg-error-50 px-6 text-center">
            <SearchX size={32} className="text-error-600" />
            <h2 className="mt-4 font-heading text-2xl font-semibold text-primary-900">Bookings unavailable</h2>
            <p className="mt-2 max-w-md text-sm text-text-secondary">{message}</p>
            <button type="button" onClick={onRetry} className="mt-5 inline-flex min-h-11 items-center rounded-md bg-primary px-5 text-sm font-semibold text-white hover:bg-primary-800">
                Try again
            </button>
        </div>
    )
}

function EmptyState() {
    return (
        <div className="flex min-h-72 flex-col items-center justify-center rounded-xl border border-dashed bg-white px-6 text-center">
            <span className="flex size-14 items-center justify-center rounded-full bg-gold-100 text-gold-600">
                <CalendarDays size={27} />
            </span>
            <h2 className="mt-4 font-heading text-2xl font-semibold text-primary-900">No stays booked yet</h2>
            <p className="mt-2 max-w-md text-sm text-text-secondary">Find a stay that feels right for your next trip.</p>
            <Link to="/" className="mt-5 inline-flex min-h-11 items-center rounded-md bg-primary px-5 text-sm font-semibold text-white hover:bg-primary-800">
                Browse hotels
            </Link>
        </div>
    )
}
