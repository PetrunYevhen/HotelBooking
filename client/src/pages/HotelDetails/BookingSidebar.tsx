import { Bed, MapPin, ShieldCheck, Users } from "lucide-react"
import type { HotelDetailsDto } from "@/api/hotels"
import type { RoomDetailsDto } from "@/api/rooms"
import { ThemedDatePicker } from "@/components/ThemedDatePicker"
import { addDays } from "./utils"

interface BookingSidebarProps {
    hotel: HotelDetailsDto
    today: string
    checkIn: string
    checkOut: string
    guests: number
    roomsRequested: number
    nights: number
    selectedRoom: RoomDetailsDto | null
    roomSubtotal: number
    onCheckInChange: (value: string) => void
    onCheckOutChange: (value: string) => void
    onReserve: () => void
}

export function BookingSidebar(props: BookingSidebarProps) {
    const { hotel, today, checkIn, checkOut, guests, roomsRequested, nights, selectedRoom, roomSubtotal, onCheckInChange, onCheckOutChange, onReserve } = props
    return (
        <aside className="mt-6 lg:absolute lg:right-0 lg:top-0 lg:mt-0 lg:w-[360px]">
            <div className="rounded-xl border bg-white p-6 shadow-md">
                <div className="flex items-center justify-between border-b pb-4"><h2 className="font-heading text-xl font-semibold text-primary-900">Your stay</h2><a href="#rooms" className="text-xs font-semibold text-primary-700 hover:text-gold-600">Edit</a></div>
                <div className="mt-5 grid gap-3 sm:grid-cols-2 lg:grid-cols-1">
                    <DateField label="Check-in"><ThemedDatePicker name="checkIn" value={checkIn} min={today} placeholder="Add date" onChange={onCheckInChange} /></DateField>
                    <DateField label="Check-out"><ThemedDatePicker name="checkOut" value={checkOut} min={addDays(checkIn, 1)} placeholder="Add date" onChange={onCheckOutChange} /></DateField>
                </div>
                <div className="mt-3 flex items-center gap-3 rounded-md border px-3.5 py-3"><Users size={18} className="text-gold-600" /><div><p className="text-[10px] font-semibold uppercase tracking-wide text-text-muted">Your search</p><p className="text-sm font-semibold text-primary-900">{guests} {guests === 1 ? "guest" : "guests"} · {roomsRequested} {roomsRequested === 1 ? "room" : "rooms"}</p></div></div>
                <div className="mt-5 border-t pt-5">
                    <div className="flex items-center justify-between"><h3 className="font-heading text-lg font-semibold text-primary-900">Selected room</h3><a href="#rooms" className="text-xs font-semibold text-primary-700 hover:text-gold-600">Edit</a></div>
                    {selectedRoom ? <div className="mt-3 rounded-lg bg-bg-muted p-4"><p className="font-semibold text-primary-900">{selectedRoom.type}</p><p className="mt-2 flex flex-wrap gap-x-4 gap-y-1 text-xs text-text-secondary"><span className="flex items-center gap-1"><Bed size={14} />{selectedRoom.beds} beds</span><span className="flex items-center gap-1"><Users size={14} />Up to {selectedRoom.capacity}</span></p><p className="mt-2 text-xs font-medium text-success-600">Booking details confirmed before payment</p></div> : <a href="#rooms" className="mt-3 flex min-h-20 items-center justify-center rounded-lg border border-dashed px-4 text-center text-sm font-medium text-text-secondary hover:border-gold-500 hover:text-primary-900">Select an available room below</a>}
                </div>
                <div className="mt-5 border-t pt-5"><h3 className="font-heading text-lg font-semibold text-primary-900">Price breakdown</h3>{selectedRoom ? <div className="mt-3 space-y-3 text-sm"><div className="flex justify-between gap-4 text-text-secondary"><span>{selectedRoom.effectivePriceAmount} {selectedRoom.effectivePriceCurrency} × {nights} {nights === 1 ? "night" : "nights"}</span><span className="shrink-0">{roomSubtotal} {selectedRoom.effectivePriceCurrency}</span></div><div className="flex justify-between border-t pt-3 text-base font-bold text-primary-900"><span>Total</span><span>{roomSubtotal} {selectedRoom.effectivePriceCurrency}</span></div><p className="text-xs text-text-muted">Taxes and fees are included in the displayed rate.</p></div> : <p className="mt-3 text-sm text-text-secondary">Choose a room to see the total price.</p>}</div>
                <button type="button" disabled={!selectedRoom} onClick={onReserve} className="mt-5 inline-flex min-h-12 w-full items-center justify-center rounded-md bg-primary px-5 text-sm font-semibold text-white transition-colors hover:bg-primary-800 disabled:cursor-not-allowed disabled:bg-text-disabled">Reserve now</button>
                <p className="mt-4 flex items-center justify-center gap-2 text-xs text-text-muted"><ShieldCheck size={15} className="text-gold-600" />Secure booking</p>
            </div>
            <section id="location" className="mt-6 scroll-mt-24 rounded-xl border bg-white p-5 shadow-sm"><div className="flex items-center justify-between gap-3"><div><p className="section-eyebrow">Location</p><h2 className="font-heading text-xl font-semibold text-primary-900">Map &amp; location</h2></div><MapPin size={21} className="shrink-0 text-gold-600" /></div><p className="mt-3 text-xs text-text-secondary">{hotel.street}, {hotel.postalCode}, {hotel.city}, {hotel.country}</p><div className="mt-4 flex min-h-44 items-center justify-center rounded-lg bg-primary-900 px-4 text-center text-white"><div><MapPin size={27} className="mx-auto text-gold-400" /><p className="mt-2 font-heading text-lg font-semibold text-white">{hotel.city}, {hotel.country}</p><p className="mt-1 text-xs text-white/65">Explore the area around your stay</p></div></div></section>
        </aside>
    )
}

function DateField({ label, children }: { label: string; children: React.ReactNode }) {
    return <div className="rounded-md border px-3.5 py-2.5 focus-within:border-gold-500 focus-within:ring-3 focus-within:ring-gold-500/15"><p className="mb-1 text-[10px] font-semibold uppercase tracking-wide text-text-muted">{label}</p>{children}</div>
}
