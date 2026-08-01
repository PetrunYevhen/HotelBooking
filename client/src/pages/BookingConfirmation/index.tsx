import { useEffect, useState, type ReactNode } from "react"
import { AlertCircle, BedDouble, CalendarDays, CheckCircle2, CircleDollarSign, Clock3, MapPin, Users } from "lucide-react"
import { Link, useParams } from "react-router-dom"
import { getBookingById, type BookingDto } from "@/api/bookings"
import { getHotelById, type HotelDetailsDto } from "@/api/hotels"
import { getRoomsByHotelId, type RoomDetailsDto } from "@/api/rooms"
import { roomImages } from "@/pages/RoomDetails/constants"
import { BookingProgress } from "@/pages/Checkout/BookingProgress"
import { formatDate, formatMoney } from "@/pages/Checkout/constants"

export function BookingConfirmation() {
    const { bookingId } = useParams<{ bookingId: string }>()
    const [booking, setBooking] = useState<BookingDto | null>(null)
    const [hotel, setHotel] = useState<HotelDetailsDto | null>(null)
    const [room, setRoom] = useState<RoomDetailsDto | null>(null)
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        if (!bookingId) { setError("This booking confirmation link is incomplete."); setIsLoading(false); return }
        let isCurrent = true
        setIsLoading(true); setError(null)
        getBookingById(bookingId)
            .then(async (bookingResult) => {
                const [hotelResult, rooms] = await Promise.all([getHotelById(bookingResult.hotelId), getRoomsByHotelId(bookingResult.hotelId, bookingResult.checkInDate.slice(0, 10))])
                if (!isCurrent) return
                setBooking(bookingResult); setHotel(hotelResult); setRoom(rooms.find((item) => item.roomId === bookingResult.roomId) ?? null)
            })
            .catch(() => { if (isCurrent) setError("We could not load this booking confirmation.") })
            .finally(() => { if (isCurrent) setIsLoading(false) })
        return () => { isCurrent = false }
    }, [bookingId])

    if (isLoading) return <ConfirmationSkeleton />
    if (error || !booking || !hotel) return <ConfirmationError message={error || "This booking could not be found."} />

    const checkIn = booking.checkInDate.slice(0, 10)
    const checkOut = booking.checkOutDate.slice(0, 10)
    const hotelUrl = `/hotels/${booking.hotelId}?checkIn=${encodeURIComponent(checkIn)}&checkOut=${encodeURIComponent(checkOut)}&guests=${booking.guestsCount ?? 1}`
    const isPending = booking.status === "Pending"
    const statusText = isPending ? "Awaiting payment" : booking.status

    return <div className="stayora-container py-8 md:py-12">
        <BookingProgress current="confirmation" />
        <div className="mx-auto mt-10 max-w-3xl text-center"><span className={`mx-auto flex size-16 items-center justify-center rounded-full ${isPending ? "bg-warning-100 text-warning-600" : "bg-success-100 text-success-600"}`}>{isPending ? <Clock3 size={32} /> : <CheckCircle2 size={32} />}</span><p className="section-eyebrow mt-5">Booking created</p><h1 className="mt-1 font-heading text-4xl font-semibold text-primary-900 md:text-5xl">{isPending ? "Your stay is reserved" : "Your booking is confirmed"}</h1><p className="mx-auto mt-3 max-w-xl text-text-secondary">{isPending ? "Your reservation has been created and is awaiting payment confirmation. We’ll send the final confirmation to your email once payment is completed." : "We’ve sent your confirmation details to your email."}</p></div>
        <div className="mx-auto mt-9 grid max-w-4xl gap-6 lg:grid-cols-[minmax(0,1fr)_320px]">
            <section className="stayora-card"><div className="flex items-start justify-between gap-4"><div><p className="text-sm text-text-muted">Booking reference</p><p className="mt-1 font-mono text-sm font-semibold text-primary-900">{booking.id}</p></div><span className={`rounded-full px-3 py-1 text-xs font-semibold ${isPending ? "bg-warning-100 text-warning-600" : "bg-success-100 text-success-600"}`}>{statusText}</span></div><div className="mt-6 flex gap-4 border-t pt-6"><img src={roomImages[0]} alt={room?.type ?? "Room"} className="size-20 rounded-lg object-cover" /><div><h2 className="font-heading text-xl font-semibold text-primary-900">{hotel.name}</h2><p className="mt-1 flex items-center gap-1 text-sm text-text-secondary"><MapPin size={15} className="text-gold-600" />{hotel.city}, {hotel.country}</p><p className="mt-2 text-sm font-medium text-primary-800">{room?.type ?? "Selected room"}</p></div></div><div className="mt-6 grid gap-4 border-t pt-6 sm:grid-cols-2"><Detail icon={<CalendarDays size={18} />} label="Check-in" value={formatDate(checkIn)} /><Detail icon={<CalendarDays size={18} />} label="Check-out" value={formatDate(checkOut)} /><Detail icon={<Users size={18} />} label="Guests" value={`${booking.guestsCount ?? 1} guest${(booking.guestsCount ?? 1) === 1 ? "" : "s"}`} /><Detail icon={<BedDouble size={18} />} label="Room" value={room?.type ?? "Selected room"} /></div></section>
            <aside className="rounded-xl border bg-white p-6 shadow-md"><p className="section-eyebrow">Payment summary</p><h2 className="section-title mt-1">Amount due</h2><div className="mt-5 space-y-3 border-y py-4 text-sm"><div className="flex justify-between text-text-secondary"><span>Room and selected extras</span><span>{formatMoney(booking.totalPrice, booking.currency)}</span></div>{booking.addOns?.map((addOn) => <div key={addOn.code} className="flex justify-between gap-3 text-text-secondary"><span>{addOn.name}</span><span className="shrink-0">{formatMoney(addOn.totalPrice, addOn.currency)}</span></div>)}</div><div className="mt-4 flex items-center justify-between"><span className="font-semibold text-primary-900">Total</span><span className="font-heading text-2xl font-semibold text-primary-900">{formatMoney(booking.totalPrice, booking.currency)}</span></div><p className="mt-4 flex gap-2 rounded-lg bg-warning-50 p-3 text-xs leading-5 text-warning-600"><CircleDollarSign size={16} className="mt-0.5 shrink-0" />Payment is not charged in this version. Your reservation remains pending until a payment method is confirmed.</p></aside>
        </div>
        <div className="mx-auto mt-8 flex max-w-4xl flex-col justify-center gap-3 sm:flex-row"><Link to="/my-booking" className="inline-flex min-h-11 items-center justify-center rounded-md bg-primary px-5 text-sm font-semibold text-white hover:bg-primary-800">View my bookings</Link><Link to={hotelUrl} className="inline-flex min-h-11 items-center justify-center rounded-md border bg-white px-5 text-sm font-semibold text-primary-800 hover:bg-muted">Back to hotel</Link></div>
    </div>
}

function Detail({ icon, label, value }: { icon: ReactNode; label: string; value: string }) { return <div className="flex gap-3"><span className="mt-0.5 text-gold-600">{icon}</span><div><p className="text-xs text-text-muted">{label}</p><p className="font-medium text-primary-900">{value}</p></div></div> }
function ConfirmationSkeleton() { return <div className="stayora-container animate-pulse py-12"><div className="mx-auto h-12 max-w-3xl rounded bg-muted" /><div className="mx-auto mt-10 h-40 max-w-xl rounded bg-muted" /><div className="mx-auto mt-9 grid max-w-4xl gap-6 lg:grid-cols-[1fr_320px]"><div className="h-72 rounded-xl bg-muted" /><div className="h-72 rounded-xl bg-muted" /></div></div> }
function ConfirmationError({ message }: { message: string }) { return <div className="stayora-container py-20 text-center"><AlertCircle size={32} className="mx-auto text-warning-600" /><h1 className="mt-4 font-heading text-3xl font-semibold text-primary-900">Confirmation unavailable</h1><p className="mt-3 text-text-secondary">{message}</p><Link to="/my-booking" className="mt-6 inline-flex min-h-11 items-center rounded-md bg-primary px-5 text-sm font-semibold text-white">View my bookings</Link></div> }
