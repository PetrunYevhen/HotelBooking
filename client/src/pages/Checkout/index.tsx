import { useEffect, useMemo, useState, type FormEvent, type ReactNode } from "react"
import { AlertCircle, BedDouble, CalendarDays, ChevronLeft, CreditCard, LockKeyhole, MapPin, ShieldCheck, Users } from "lucide-react"
import { Link, useNavigate, useSearchParams } from "react-router-dom"
import { createBooking, getBookingQuote, type BookingQuoteDto, type CreateBookingAddOnRequest } from "@/api/bookings"
import { getHotelAddOns, getHotelById, type HotelAddOnDto, type HotelDetailsDto } from "@/api/hotels"
import { getRoomsByHotelId, type RoomDetailsDto } from "@/api/rooms"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { hasAccessToken } from "@/lib/api-client"
import { addDays, calculateNights, toLocalDateValue } from "@/pages/HotelDetails/utils"
import { roomImages } from "@/pages/RoomDetails/constants"
import { BookingProgress } from "./BookingProgress"
import { formatDate, formatMoney } from "./constants"

type GuestForm = { firstName: string; lastName: string; email: string; phoneNumber: string; country: string; arrivalTime: string; purpose: string; specialRequest: string; billingAddress: "same" | "different" }
const initialGuestForm: GuestForm = { firstName: "", lastName: "", email: "", phoneNumber: "", country: "", arrivalTime: "", purpose: "Leisure", specialRequest: "", billingAddress: "same" }

export function Checkout() {
    const [searchParams] = useSearchParams()
    const navigate = useNavigate()
    const today = toLocalDateValue(new Date())
    const hotelId = searchParams.get("hotelId") ?? ""
    const roomId = searchParams.get("roomId") ?? ""
    const checkIn = searchParams.get("checkIn") ?? today
    const checkOut = searchParams.get("checkOut") ?? addDays(checkIn, 1)
    const requestedGuests = Math.max(1, Number(searchParams.get("guests") ?? 1))
    const datesAreValid = /^\d{4}-\d{2}-\d{2}$/.test(checkIn) && /^\d{4}-\d{2}-\d{2}$/.test(checkOut) && checkOut > checkIn
    const [hotel, setHotel] = useState<HotelDetailsDto | null>(null)
    const [room, setRoom] = useState<RoomDetailsDto | null>(null)
    const [addOnOptions, setAddOnOptions] = useState<HotelAddOnDto[]>([])
    const [selectedAddOnIds, setSelectedAddOnIds] = useState<Set<string>>(new Set())
    const [quote, setQuote] = useState<BookingQuoteDto | null>(null)
    const [isLoading, setIsLoading] = useState(true)
    const [isQuoting, setIsQuoting] = useState(false)
    const [loadError, setLoadError] = useState<string | null>(null)
    const [quoteError, setQuoteError] = useState<string | null>(null)
    const [form, setForm] = useState<GuestForm>(initialGuestForm)
    const [isSubmitting, setIsSubmitting] = useState(false)
    const [submitError, setSubmitError] = useState<string | null>(null)
    const nights = calculateNights(checkIn, checkOut)
    const guests = room ? Math.min(requestedGuests, room.capacity) : requestedGuests
    const returnToRoom = hotelId && roomId ? `/hotels/${hotelId}/rooms/${roomId}?checkIn=${encodeURIComponent(checkIn)}&checkOut=${encodeURIComponent(checkOut)}&guests=${guests}` : "/search"
    const selectedOptions = useMemo(() => addOnOptions.filter((option) => selectedAddOnIds.has(option.hotelAddOnId)), [addOnOptions, selectedAddOnIds])
    const selectedAddOns = useMemo<CreateBookingAddOnRequest[]>(() => selectedOptions.map((option) => ({ hotelAddOnId: option.hotelAddOnId, quantity: 1 })), [selectedOptions])
    const quoteLines = useMemo(() => new Map((quote?.addOns ?? []).map((line) => [line.hotelAddOnId, line])), [quote])

    useEffect(() => {
        if (!hotelId || !roomId || !datesAreValid) { setLoadError("This booking link is incomplete or has invalid dates."); setIsLoading(false); return }
        let isCurrent = true
        setIsLoading(true); setLoadError(null)
        Promise.all([getHotelById(hotelId), getRoomsByHotelId(hotelId, checkIn), getHotelAddOns(hotelId)])
            .then(([hotelResult, rooms, addOns]) => { if (isCurrent) { setHotel(hotelResult); setRoom(rooms.find((item) => item.roomId === roomId) ?? null); setAddOnOptions(addOns) } })
            .catch(() => { if (isCurrent) setLoadError("We could not load this room for the selected dates.") })
            .finally(() => { if (isCurrent) setIsLoading(false) })
        return () => { isCurrent = false }
    }, [hotelId, roomId, checkIn, datesAreValid])

    useEffect(() => {
        if (!room || !hotel || !datesAreValid) return
        let isCurrent = true
        setIsQuoting(true); setQuoteError(null)
        getBookingQuote({ hotelId: room.hotelId, roomId: room.roomId, checkIn, checkOut, guestCount: guests, addOns: selectedAddOns })
            .then((result) => { if (isCurrent) setQuote(result) })
            .catch(() => { if (isCurrent) { setQuote(null); setQuoteError("We could not update the quote.") } })
            .finally(() => { if (isCurrent) setIsQuoting(false) })
        return () => { isCurrent = false }
    }, [room, hotel, datesAreValid, checkIn, checkOut, guests, selectedAddOns])

    function updateForm<K extends keyof GuestForm>(key: K, value: GuestForm[K]) { setForm((current) => ({ ...current, [key]: value })) }
    function toggleAddOn(hotelAddOnId: string) { setSelectedAddOnIds((current) => { const next = new Set(current); next.has(hotelAddOnId) ? next.delete(hotelAddOnId) : next.add(hotelAddOnId); return next }) }
    function pricingLabel(type: number) { return type === 1 ? "per stay" : type === 2 ? `per guest (${guests} guests)` : `per guest per night (${guests} × ${nights})` }

    async function submit(event: FormEvent<HTMLFormElement>) {
        event.preventDefault()
        if (!room || !hotel || !quote || isSubmitting || isQuoting) return
        setIsSubmitting(true); setSubmitError(null)
        const stayNotes = [form.country && `Country: ${form.country}`, form.arrivalTime && `Estimated arrival: ${form.arrivalTime}`, form.purpose && `Purpose of stay: ${form.purpose}`, form.specialRequest.trim()].filter(Boolean).join("\n")
        try {
            if (!hasAccessToken()) { navigate(`/login?returnTo=${encodeURIComponent(`${location.pathname}${location.search}`)}`); return }
            const bookingId = await createBooking({ hotelId: room.hotelId, roomId: room.roomId, checkIn, checkOut, guestCount: guests, firstName: form.firstName.trim(), lastName: form.lastName.trim(), email: form.email.trim(), phoneNumber: form.phoneNumber.trim(), specialRequest: stayNotes || undefined, addOns: selectedAddOns })
            navigate(`/booking-confirmation/${bookingId}`)
        } catch { setSubmitError("We could not create your booking. Please check the details and try again.") }
        finally { setIsSubmitting(false) }
    }

    if (isLoading) return <CheckoutSkeleton />
    if (loadError || !hotel || !room) return <InvalidCheckout message={loadError || "This room is no longer available."} returnTo={returnToRoom} />

    return <div className="stayora-container py-8 md:py-12">
        <BookingProgress current="payment" />
        <div className="mt-9 flex items-center gap-2 text-sm text-text-muted"><Link to={returnToRoom} className="inline-flex items-center gap-1 hover:text-primary-800"><ChevronLeft size={16} />Back to room</Link><span>•</span><span>Secure checkout</span></div>
        <div className="mt-5 grid gap-7 xl:grid-cols-[minmax(0,1fr)_370px] xl:items-start">
            <form id="checkout-form" onSubmit={submit} className="space-y-6">
                <section className="stayora-card"><p className="section-eyebrow">Step 1</p><h1 className="section-title mt-1">Guest information</h1><p className="mt-1 text-sm text-text-secondary">We’ll use these details for your booking confirmation.</p><div className="mt-5 grid gap-4 sm:grid-cols-2"><TextField label="First name" value={form.firstName} onChange={(value) => updateForm("firstName", value)} autoComplete="given-name" /><TextField label="Last name" value={form.lastName} onChange={(value) => updateForm("lastName", value)} autoComplete="family-name" /><TextField label="Email address" type="email" value={form.email} onChange={(value) => updateForm("email", value)} autoComplete="email" /><TextField label="Phone number" type="tel" value={form.phoneNumber} onChange={(value) => updateForm("phoneNumber", value)} autoComplete="tel" /><div className="sm:col-span-2"><TextField label="Country / region" value={form.country} onChange={(value) => updateForm("country", value)} autoComplete="country-name" /></div></div></section>
                <section className="stayora-card"><p className="section-eyebrow">Step 2</p><h2 className="section-title mt-1">Stay details</h2><div className="mt-5 grid gap-4 sm:grid-cols-2"><TextField label="Estimated arrival time" type="time" value={form.arrivalTime} onChange={(value) => updateForm("arrivalTime", value)} required={false} /><div><Label htmlFor="stay-purpose">Purpose of stay</Label><select id="stay-purpose" value={form.purpose} onChange={(event) => updateForm("purpose", event.target.value)} className="mt-1 h-11 w-full rounded-md border bg-white px-3 text-sm text-primary-900"><option>Leisure</option><option>Business</option><option>Celebration</option><option>Other</option></select></div></div><div className="mt-4"><Label htmlFor="special-request">Special requests <span className="font-normal text-text-muted">(optional)</span></Label><textarea id="special-request" value={form.specialRequest} onChange={(event) => updateForm("specialRequest", event.target.value)} maxLength={500} rows={4} placeholder="Dietary requests, accessibility needs, or anything else we should know" className="mt-1 w-full rounded-md border bg-white px-3 py-2 text-sm text-primary-900 outline-none transition focus:border-brand-blue-600 focus:ring-3 focus:ring-brand-blue-100" /></div></section>
                <section className="stayora-card"><p className="section-eyebrow">Enhance your stay</p><h2 className="section-title mt-1">Optional add-ons</h2><div className="mt-5 space-y-3">{addOnOptions.map((option) => { const selected = selectedAddOnIds.has(option.hotelAddOnId); const line = quoteLines.get(option.hotelAddOnId); return <div key={option.hotelAddOnId} className={`rounded-lg border p-4 transition ${selected ? "border-brand-blue-600 bg-brand-blue-50/40" : "bg-white"}`}><div className="flex gap-3"><input id={option.hotelAddOnId} type="checkbox" checked={selected} onChange={() => toggleAddOn(option.hotelAddOnId)} className="mt-1 size-4 accent-primary" /><div className="min-w-0 flex-1"><label htmlFor={option.hotelAddOnId} className="cursor-pointer text-sm font-semibold text-primary-900">{option.name}</label><p className="mt-0.5 text-xs text-text-secondary">{option.description}</p><p className="mt-2 text-sm font-semibold text-primary-800">{selected && line ? formatMoney(line.lineTotal, line.currency) : formatMoney(option.priceAmount, option.priceCurrency)} <span className="font-normal text-text-muted">{pricingLabel(option.pricingType)}</span></p></div></div></div> })}</div></section>
                <section className="stayora-card"><p className="section-eyebrow">Step 3</p><h2 className="section-title mt-1">Payment method</h2><p className="mt-1 text-sm text-text-secondary">Payment details are collected for the booking flow; no charge is made in this version.</p><div className="mt-5 grid gap-3 sm:grid-cols-3"><PaymentOption label="Credit card" icon={<CreditCard size={20} />} active /><PaymentOption label="Apple Pay" icon={<span className="font-bold"></span>} /><PaymentOption label="Google Pay" icon={<span className="font-bold">G</span>} /></div><div className="mt-5 rounded-lg border bg-bg-warm p-4"><p className="text-sm font-semibold text-primary-900">Billing address</p><div className="mt-3 flex flex-wrap gap-4 text-sm text-text-secondary"><label className="flex items-center gap-2"><input type="radio" name="billing-address" checked={form.billingAddress === "same"} onChange={() => updateForm("billingAddress", "same")} />Same as guest details</label><label className="flex items-center gap-2"><input type="radio" name="billing-address" checked={form.billingAddress === "different"} onChange={() => updateForm("billingAddress", "different")} />Use a different address</label></div></div><div className="mt-5 flex gap-3 rounded-lg bg-success-50 p-4 text-sm text-success-600"><ShieldCheck className="mt-0.5 shrink-0" size={19} /><p>Your reservation is created securely. It will remain pending until payment is confirmed.</p></div>{submitError && <p role="alert" className="mt-4 flex gap-2 text-sm text-error-600"><AlertCircle size={17} />{submitError}</p>}</section>
            </form>
            <aside className="xl:sticky xl:top-24"><div className="overflow-hidden rounded-xl border bg-white shadow-md"><img src={roomImages[0]} alt={room.type} className="h-40 w-full object-cover" /><div className="p-5"><p className="text-xs font-semibold uppercase tracking-[0.08em] text-gold-600">Booking summary</p><h2 className="mt-1 font-heading text-xl font-semibold text-primary-900">{hotel.name}</h2><p className="mt-1 flex items-center gap-1 text-sm text-text-secondary"><MapPin size={15} className="text-gold-600" />{hotel.city}, {hotel.country}</p><div className="mt-5 space-y-3 border-y py-4 text-sm"><SummaryRow icon={<BedDouble size={16} />} label="Room" value={room.type} /><SummaryRow icon={<CalendarDays size={16} />} label="Stay" value={`${formatDate(checkIn)} – ${formatDate(checkOut)}`} /><SummaryRow icon={<Users size={16} />} label="Guests" value={`${guests} guest${guests === 1 ? "" : "s"}, ${nights} night${nights === 1 ? "" : "s"}`} /></div><div className="mt-5 space-y-3 text-sm"><div className="flex justify-between gap-4 text-text-secondary"><span>Room total</span><span className="shrink-0">{quote ? formatMoney(quote.baseTotal, quote.currency) : "—"}</span></div>{quote?.addOns.map((line) => <div key={line.hotelAddOnId} className="flex justify-between gap-4 text-text-secondary"><span>{line.name}</span><span className="shrink-0">{formatMoney(line.lineTotal, line.currency)}</span></div>)}<div className="flex justify-between gap-4 text-text-secondary"><span>Taxes &amp; fees</span><span>Included</span></div><div className="flex justify-between border-t pt-4 text-base font-bold text-primary-900"><span>Total</span><span>{quote ? formatMoney(quote.total, quote.currency) : isQuoting ? "Updating…" : "—"}</span></div>{quoteError && <p role="alert" className="text-sm text-error-600">{quoteError}</p>}</div><Button type="submit" form="checkout-form" disabled={isSubmitting || isQuoting || !quote} className="mt-5 w-full">{isSubmitting ? "Creating booking…" : isQuoting ? "Updating quote…" : "Confirm booking"}</Button><p className="mt-3 flex items-start gap-2 text-xs text-text-muted"><LockKeyhole size={15} className="mt-0.5 shrink-0 text-gold-600" />By confirming, you accept the hotel’s cancellation policy and terms.</p></div></div></aside>
        </div>
    </div>
}

function TextField({ label, value, onChange, type = "text", autoComplete, required = true }: { label: string; value: string; onChange: (value: string) => void; type?: string; autoComplete?: string; required?: boolean }) { const id = `checkout-${label.toLowerCase().replace(/[^a-z0-9]+/g, "-")}`; return <div><Label htmlFor={id}>{label}</Label><Input id={id} type={type} value={value} onChange={(event) => onChange(event.target.value)} autoComplete={autoComplete} required={required} className="mt-1" /></div> }
function PaymentOption({ label, icon, active = false }: { label: string; icon: ReactNode; active?: boolean }) { return <button type="button" aria-pressed={active} className={`flex min-h-16 items-center justify-center gap-2 rounded-lg border text-sm font-semibold ${active ? "border-brand-blue-600 bg-brand-blue-50 text-primary-900" : "bg-white text-text-secondary hover:bg-muted"}`}>{icon}{label}</button> }
function SummaryRow({ icon, label, value }: { icon: ReactNode; label: string; value: string }) { return <div className="flex gap-3"><span className="mt-0.5 text-gold-600">{icon}</span><div><p className="text-xs text-text-muted">{label}</p><p className="font-medium text-primary-900">{value}</p></div></div> }
function CheckoutSkeleton() { return <div className="stayora-container animate-pulse py-12"><div className="mx-auto h-12 max-w-3xl rounded bg-muted" /><div className="mt-9 grid gap-7 xl:grid-cols-[1fr_370px]"><div className="h-[860px] rounded-xl bg-muted" /><div className="h-[560px] rounded-xl bg-muted" /></div></div> }
function InvalidCheckout({ message, returnTo }: { message: string; returnTo: string }) { return <div className="stayora-container py-20 text-center"><AlertCircle size={32} className="mx-auto text-warning-600" /><h1 className="mt-4 font-heading text-3xl font-semibold text-primary-900">We can’t open this checkout</h1><p className="mx-auto mt-3 max-w-lg text-text-secondary">{message}</p><Link to={returnTo} className="mt-6 inline-flex min-h-11 items-center rounded-md bg-primary px-5 text-sm font-semibold text-white">Back to room</Link></div> }
