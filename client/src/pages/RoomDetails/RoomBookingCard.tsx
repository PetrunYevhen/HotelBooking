import { CalendarDays, CheckCircle2, Crown, ShieldCheck, Users } from "lucide-react"
import type { RoomDetailsDto } from "@/api/rooms"

interface RoomBookingCardProps {
    room: RoomDetailsDto
    image: string
    checkIn: string
    checkOut: string
    guests: number
    nights: number
    total: number
    onBook: () => void
}

export function RoomBookingCard({ room, image, checkIn, checkOut, guests, nights, total, onBook }: RoomBookingCardProps) {
    return <aside className="mt-6 lg:absolute lg:right-0 lg:top-0 lg:mt-0 lg:w-[360px]"><div className="overflow-hidden rounded-xl border bg-white shadow-md"><div className="relative h-44"><img src={image} alt={room.type} className="h-full w-full object-cover" /><span className="absolute right-3 top-3 rounded-full bg-white px-3 py-1.5 text-[11px] font-semibold text-primary-900 shadow-sm">Best price guarantee</span></div><div className="p-6"><h2 className="font-heading text-xl font-semibold text-primary-900">Your stay</h2><div className="mt-4 grid grid-cols-2 gap-4 border-y py-4"><StayDate label="Check-in" value={checkIn} /><StayDate label="Check-out" value={checkOut} /></div><div className="flex items-center gap-2 border-b py-4 text-sm text-text-secondary"><Users size={17} className="text-gold-600" />{guests} {guests === 1 ? "guest" : "guests"}, 1 room</div><div className="mt-5"><div className="flex items-start justify-between"><h3 className="font-heading text-lg font-semibold text-primary-900">Price summary</h3><div className="text-right"><p className="font-heading text-xl font-semibold text-primary-900">{room.effectivePriceAmount} {room.effectivePriceCurrency}</p><p className="text-[11px] text-text-muted">Avg. per night</p></div></div><div className="mt-4 space-y-2 border-b pb-4 text-sm text-text-secondary"><div className="flex justify-between"><span>{nights} {nights === 1 ? "night" : "nights"}</span><span>{room.effectivePriceAmount * nights} {room.effectivePriceCurrency}</span></div><div className="flex justify-between"><span>Taxes &amp; fees</span><span>Included</span></div></div><div className="mt-4 flex items-center justify-between"><span className="font-semibold text-primary-900">Total</span><span className="font-heading text-2xl font-semibold text-primary-900">{total} {room.effectivePriceCurrency}</span></div></div><div className="mt-5 rounded-lg border border-gold-500/30 bg-gold-100/50 p-4"><div className="flex gap-3"><Crown size={22} className="shrink-0 text-gold-600" /><div><p className="text-sm font-semibold text-primary-900">Join Stayora Membership</p><p className="mt-1 text-xs leading-4 text-text-secondary">Save up to 10% on this booking.</p><a href="#membership" className="mt-2 inline-block text-xs font-semibold text-primary-700 underline underline-offset-2">Join now</a></div></div></div><button type="button" onClick={onBook} className="mt-5 inline-flex min-h-12 w-full items-center justify-center rounded-md bg-primary px-5 text-sm font-semibold text-white transition-colors hover:bg-primary-800">Book this room</button><div className="mt-4 space-y-2.5 text-xs text-text-secondary"><p className="flex items-center gap-2"><CheckCircle2 size={16} className="text-success-600" />Instant confirmation</p><p className="flex items-center gap-2"><ShieldCheck size={16} className="text-gold-600" />Secure booking and payment</p></div></div></div><section className="mt-6 rounded-xl border bg-white p-5 shadow-sm"><h2 className="font-heading text-lg font-semibold text-primary-900">Need help?</h2><p className="mt-2 text-sm text-text-secondary">Our travel experts are here for you.</p><a href="#contact" className="mt-3 inline-flex items-center gap-2 text-sm font-semibold text-primary-700 hover:text-gold-600"><CalendarDays size={16} />Contact support</a></section></aside>
}

function StayDate({ label, value }: { label: string; value: string }) {
    return <div><p className="text-[10px] font-semibold uppercase tracking-wide text-text-muted">{label}</p><p className="mt-1 text-sm font-semibold text-primary-900">{new Date(`${value}T00:00:00`).toLocaleDateString(undefined, { month: "short", day: "numeric", year: "numeric" })}</p></div>
}
