import { CalendarDays, MapPin, SlidersHorizontal, Users } from "lucide-react"
import { Link } from "react-router-dom"
import { formatDate } from "./data"

interface SearchSummaryProps { destination?: string; checkIn?: string; checkOut?: string; guests: number; rooms: number; childrenCount: number }

export function SearchSummary({ destination, checkIn, checkOut, guests, rooms, childrenCount }: SearchSummaryProps) {
    return <section className="rounded-xl border bg-white p-4 shadow-sm md:flex md:items-center md:justify-between md:gap-6 md:p-5"><div className="grid flex-1 gap-4 sm:grid-cols-2 xl:grid-cols-4"><SearchDetail icon={<MapPin size={18} />} label="Destination" value={destination || "Anywhere"} /><SearchDetail icon={<CalendarDays size={18} />} label="Dates" value={`${formatDate(checkIn)} – ${formatDate(checkOut)}`} /><SearchDetail icon={<Users size={18} />} label="Guests" value={`${guests} ${guests === 1 ? "guest" : "guests"}${childrenCount ? ` · ${childrenCount} ${childrenCount === 1 ? "child" : "children"}` : ""}`} /><SearchDetail icon={<SlidersHorizontal size={18} />} label="Rooms" value={`${rooms} ${rooms === 1 ? "room" : "rooms"}`} /></div><Link to="/#search" className="mt-4 inline-flex min-h-11 w-full shrink-0 items-center justify-center rounded-md bg-primary px-5 text-sm font-semibold text-white transition-colors hover:bg-primary-800 md:mt-0 md:w-auto">Change search</Link></section>
}

function SearchDetail({ icon, label, value }: { icon: React.ReactNode; label: string; value: string }) {
    return <div className="flex min-w-0 items-center gap-3"><span className="inline-flex size-9 shrink-0 items-center justify-center rounded-full bg-gold-100 text-gold-600">{icon}</span><span className="min-w-0"><span className="block text-[10px] font-semibold uppercase tracking-wide text-text-muted">{label}</span><span className="block truncate text-sm font-semibold text-primary-900">{value}</span></span></div>
}
