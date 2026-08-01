import { Bed, Check, Users } from "lucide-react"
import { Link, useNavigate } from "react-router-dom"
import type { RoomDetailsDto } from "@/api/rooms"
import { Button } from "@/components/ui/button"

interface RoomCardProps {
    room: RoomDetailsDto
    checkIn: string
    checkOut: string
    initialGuestCount?: number
    isSelected?: boolean
    onSelect?: (room: RoomDetailsDto) => void
}

const roomImages = [
    "https://images.unsplash.com/photo-1590490360182-c33d57733427?auto=format&fit=crop&w=800&q=80",
    "https://images.unsplash.com/photo-1611892440504-42a792e24d32?auto=format&fit=crop&w=800&q=80",
    "https://images.unsplash.com/photo-1566665797739-1674de7a421a?auto=format&fit=crop&w=800&q=80",
]

function roomImageIndex(roomId: string) {
    return Array.from(roomId).reduce((sum, character) => sum + character.charCodeAt(0), 0) % roomImages.length
}

export function RoomCard({ room, checkIn, checkOut, initialGuestCount = 1, isSelected = false, onSelect }: RoomCardProps) {
    const navigate = useNavigate()
    const stayNights = Math.max(1, Math.round((new Date(`${checkOut}T00:00:00`).getTime() - new Date(`${checkIn}T00:00:00`).getTime()) / 86_400_000))
    const totalForStay = room.effectivePriceAmount * stayNights
    const detailsSearch = new URLSearchParams({ checkIn, checkOut, guests: String(initialGuestCount) }).toString()
    const detailsUrl = `/hotels/${room.hotelId}/rooms/${room.roomId}?${detailsSearch}`

    function openDetails(event: React.MouseEvent<HTMLElement>) {
        const target = event.target as HTMLElement
        if (target.closest("a, button, input, label, select, textarea, form")) return
        navigate(detailsUrl)
    }

    return <article id={`room-${room.roomId}`} onClick={openDetails} className={`flex cursor-pointer scroll-mt-24 flex-col overflow-hidden rounded-xl border bg-card shadow-sm transition-shadow hover:shadow-md ${isSelected ? "border-gold-500 ring-2 ring-gold-500/15" : ""}`}>
        <div className="flex flex-col md:flex-row"><img src={roomImages[roomImageIndex(room.roomId)]} alt={room.type} className="h-48 w-full shrink-0 object-cover md:h-auto md:w-52" /><div className="min-w-0 flex-1 p-5"><div className="flex flex-wrap items-center gap-x-2 gap-y-1"><h3 className="font-heading text-xl font-semibold text-primary-900">{room.type}</h3><span className="rounded bg-bg-muted px-2 py-0.5 text-xs font-medium text-text-secondary">Room {room.roomNumber}</span></div><div className="mt-3 flex flex-wrap gap-2 text-xs text-text-secondary"><span className="inline-flex items-center gap-1 rounded-full bg-bg-muted px-2.5 py-1"><Bed size={14} /> {room.beds} {room.beds === 1 ? "bed" : "beds"}</span><span className="inline-flex items-center gap-1 rounded-full bg-bg-muted px-2.5 py-1"><Users size={14} /> Up to {room.capacity} guests</span></div>{room.description && <p className="mt-3 text-sm leading-5 text-text-secondary">{room.description}</p>}<p className="mt-3 flex items-center gap-1.5 text-xs font-semibold text-success-600"><Check size={15} /> Continue to secure checkout</p><Link to={detailsUrl} className="mt-3 inline-flex text-xs font-semibold text-primary-700 hover:text-gold-600">View room details →</Link></div><div className="flex shrink-0 items-center justify-between gap-4 border-t bg-bg-warm p-5 md:w-44 md:flex-col md:items-end md:justify-center md:border-t-0 md:border-l md:bg-white"><div className="md:text-right"><p className="text-xs text-text-secondary">{room.effectivePriceAmount} {room.effectivePriceCurrency} / night</p><p className="mt-1 font-heading text-xl font-semibold text-primary-900">{totalForStay} {room.effectivePriceCurrency}</p><p className="text-xs text-text-muted">{stayNights} {stayNights === 1 ? "night" : "nights"}</p></div><Button type="button" disabled={!room.isActive} onClick={() => { if (onSelect) { onSelect(room); return } navigate(`/checkout?hotelId=${encodeURIComponent(room.hotelId)}&roomId=${encodeURIComponent(room.roomId)}&${detailsSearch}`) }} className="shrink-0">{isSelected ? "Selected" : onSelect ? "Select room" : "Book room"}</Button></div></div>
    </article>
}
