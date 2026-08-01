import { Bed, Check, Clock, MapPin, ShieldCheck, Sparkles, Users, Wifi } from "lucide-react"
import type { FacilityDto } from "@/api/hotels"
import type { RoomDetailsDto } from "@/api/rooms"
import { getFacilityIcon } from "@/pages/HotelDetails/constants"

export function RoomFeatures({ room }: { room: RoomDetailsDto }) {
    const features = [
        [Bed, `${room.beds} ${room.beds === 1 ? "King bed" : "beds"}`, "Sleep comfortably"],
        [Users, `Up to ${room.capacity} guests`, "Max occupancy"],
        [Sparkles, "Thoughtful design", "Comfortable stay"],
        [Wifi, "Free Wi-Fi", "Stay connected"],
    ]
    return <section className="grid grid-cols-2 divide-x divide-y rounded-xl border sm:grid-cols-4">{features.map(([Icon, title, caption], index) => { const FeatureIcon = Icon as typeof Bed; return <div key={String(title)} className={`flex min-h-28 flex-col items-center justify-center p-4 text-center ${index < 2 ? "sm:border-b-0" : ""}`}><FeatureIcon size={24} strokeWidth={1.5} className="text-gold-600" /><p className="mt-2 text-xs font-semibold text-primary-900">{title as string}</p><p className="mt-1 text-[11px] text-text-muted">{caption as string}</p></div> })}</section>
}

export function RoomAmenities({ facilities }: { facilities: FacilityDto[] }) {
    const amenities = facilities.length ? facilities.slice(0, 8) : ["Air conditioning", "Free Wi-Fi", "Premium linens", "Coffee & tea", "Workspace", "Daily housekeeping", "Private bathroom", "Room service"]
    return <section id="amenities" className="mt-9 scroll-mt-24"><div className="flex items-end justify-between gap-4"><div><p className="section-eyebrow">Amenities & services</p><h2 className="section-title mt-1">Everything you need for your stay</h2></div></div><div className="mt-5 grid gap-3 sm:grid-cols-2">{amenities.map((amenity) => { const name = typeof amenity === "string" ? amenity : amenity.name; const Icon = getFacilityIcon(name); return <div key={name} className="flex items-center gap-3 rounded-lg border bg-white p-4"><Icon size={20} strokeWidth={1.6} className="text-gold-600" /><span className="text-sm font-medium text-primary-900">{name}</span><Check size={16} className="ml-auto text-success-600" /></div> })}</div></section>
}

export function RoomPolicies() {
    const policies = [[Clock, "Check-in / Check-out", "Check-in from 3:00 PM · Check-out by 11:00 AM"], [ShieldCheck, "Cancellation", "Review the cancellation terms before confirmation."], [MapPin, "Hotel rules", "House rules and local information are shown during booking."]]
    return <section id="policies" className="mt-9 scroll-mt-24 rounded-xl border bg-white p-5"><h2 className="font-heading text-xl font-semibold text-primary-900">Good to know</h2><div className="mt-5 grid gap-5 md:grid-cols-3">{policies.map(([Icon, title, copy]) => { const PolicyIcon = Icon as typeof Clock; return <div key={title as string}><PolicyIcon size={21} strokeWidth={1.6} className="text-gold-600" /><h3 className="mt-2 text-sm font-semibold text-primary-900">{title as string}</h3><p className="mt-1 text-xs leading-5 text-text-secondary">{copy as string}</p></div> })}</div></section>
}
