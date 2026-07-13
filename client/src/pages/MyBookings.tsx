import { useEffect, useState } from "react"
import { Link } from "react-router-dom"
import { Calendar } from "lucide-react"
import { getBookingsByUserId, type BookingDto } from "@/api/bookings"
import { getHotelById } from "@/api/hotels"
import { GUEST_USER_ID } from "@/lib/constants"
import { ApiError } from "@/lib/api-client"

const statusStyles: Record<string, string> = {
    Pending: "bg-gray-100 text-gray-700",
    Confirmed: "bg-blue-100 text-blue-700",
    CheckedIn: "bg-purple-100 text-purple-700",
    Completed: "bg-green-100 text-green-700",
    Cancelled: "bg-red-100 text-red-700",
}

export function MyBookings() {
    const [bookings, setBookings] = useState<BookingDto[]>([])
    const [hotelNames, setHotelNames] = useState<Record<string, string>>({})
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        getBookingsByUserId(GUEST_USER_ID)
            .then(setBookings)
            .catch((reason: unknown) => {
                setError(reason instanceof ApiError ? reason.message : "Could not load bookings.")
            })
            .finally(() => setIsLoading(false))
    }, [])

    useEffect(() => {
        const uniqueHotelIds = [...new Set(bookings.map((b) => b.hotelId))]
            .filter((hotelId) => !(hotelId in hotelNames))

        if (uniqueHotelIds.length === 0) return

        void Promise.all(
            uniqueHotelIds.map(async (hotelId) => {
                try {
                    const hotel = await getHotelById(hotelId)
                    return [hotelId, hotel.name] as const
                } catch {
                    return [hotelId, "Hotel unavailable"] as const
                }
            }),
        ).then((names) => {
            setHotelNames((previous) => ({ ...previous, ...Object.fromEntries(names) }))
        })
    }, [bookings, hotelNames])

    if (isLoading) return <p>Loading...</p>

    if (error) {
        return <p role="alert" className="text-red-600">{error}</p>
    }

    if (bookings.length === 0) {
        return (
            <div className="flex flex-col items-center justify-center gap-2 py-16 text-gray-500">
                <Calendar size={32} />
                <p>You don't have any bookings yet.</p>
                <Link to="/" className="text-blue-600 text-sm">
                    Browse hotels
                </Link>
            </div>
        )
    }

    return (
        <div className="flex flex-col gap-4">
            <h1 className="text-2xl font-semibold">My Bookings</h1>

            <div className="flex flex-col gap-3">
                {bookings.map((booking) => (
                    <Link
                        key={booking.id}
                        to={`/hotels/${booking.hotelId}`}
                        className="border rounded-xl p-4 flex items-center justify-between hover:bg-gray-50"
                    >
                        <div>
                            <p className="font-medium">{hotelNames[booking.hotelId] ?? "Loading hotel..."}</p>
                            <p className="text-sm text-gray-500 mt-1">
                                {new Date(booking.checkInDate).toLocaleDateString()} —{" "}
                                {new Date(booking.checkOutDate).toLocaleDateString()}
                            </p>
                        </div>

                        <div className="text-right shrink-0">
                            <span
                                className={`px-3 py-1 rounded-full text-xs font-medium ${
                                    statusStyles[booking.status] ?? "bg-gray-100 text-gray-700"
                                }`}
                            >
                                {booking.status}
                            </span>
                            <p className="text-sm font-semibold mt-1">
                                {new Intl.NumberFormat(undefined, {
                                    style: "currency",
                                    currency: booking.currency,
                                }).format(booking.totalPrice)}
                            </p>
                        </div>
                    </Link>
                ))}
            </div>
        </div>
    )
}
