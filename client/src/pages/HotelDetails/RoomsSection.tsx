import { ChevronLeft, ChevronRight } from "lucide-react"
import type { RoomDetailsDto } from "@/api/rooms"
import { RoomCard } from "@/components/RoomCard"

interface RoomsSectionProps {
    rooms: RoomDetailsDto[]
    isLoading: boolean
    error: string | null
    checkIn: string
    checkOut: string
    nights: number
    guestsPerRoom: number
    selectedRoomId: string | null
    showAll: boolean
    currentPage: number
    totalPages: number
    totalRooms: number
    onSelect: (room: RoomDetailsDto) => void
    onShowAll: () => void
    onPageChange: (page: number) => void
}

export function RoomsSection(props: RoomsSectionProps) {
    const { rooms, isLoading, error, checkIn, checkOut, nights, guestsPerRoom, selectedRoomId, showAll, currentPage, totalPages, totalRooms, onSelect, onShowAll, onPageChange } = props
    return (
        <section id="rooms" className="mt-8 scroll-mt-24 border-t pt-8">
            <p className="section-eyebrow">Choose your stay</p>
            <div className="mt-1 flex flex-wrap items-end justify-between gap-3"><div><h2 className="section-title">Available rooms</h2><p className="mt-2 text-sm text-text-secondary">{checkIn} – {checkOut} · {nights} {nights === 1 ? "night" : "nights"}</p></div>{!isLoading && !error && <p className="text-sm text-text-muted">{totalRooms} matching {totalRooms === 1 ? "room" : "rooms"}</p>}</div>
            {isLoading && <div className="mt-5 space-y-4">{[0, 1].map((item) => <div key={item} className="h-44 animate-pulse rounded-xl bg-muted" />)}</div>}
            {error && <p role="alert" className="mt-5 rounded-lg bg-error-50 p-4 text-sm text-error-600">{error}</p>}
            {!isLoading && !error && totalRooms === 0 && <div className="mt-5 rounded-xl border bg-white p-8 text-center"><h3 className="font-heading text-xl font-semibold text-primary-900">No rooms match this guest count</h3><p className="mt-2 text-sm text-text-secondary">Try changing your search or selecting another property.</p></div>}
            {!isLoading && !error && totalRooms > 0 && <div className="mt-5 space-y-4">{rooms.map((room) => <RoomCard key={room.roomId} room={room} checkIn={checkIn} checkOut={checkOut} initialGuestCount={Math.min(guestsPerRoom, room.capacity)} isSelected={selectedRoomId === room.roomId} onSelect={onSelect} />)}</div>}
            {!isLoading && !error && !showAll && totalRooms > 3 && <button type="button" onClick={onShowAll} className="mt-6 inline-flex min-h-11 items-center justify-center rounded-md border border-gold-500 px-5 text-sm font-semibold text-primary-900 transition-colors hover:bg-gold-100">View all {totalRooms} rooms</button>}
            {!isLoading && !error && showAll && totalPages > 1 && <RoomPagination currentPage={currentPage} totalPages={totalPages} onChange={onPageChange} />}
        </section>
    )
}

function RoomPagination({ currentPage, totalPages, onChange }: { currentPage: number; totalPages: number; onChange: (page: number) => void }) {
    return <nav aria-label="Rooms pagination" className="mt-8 flex flex-wrap items-center justify-center gap-2"><button type="button" aria-label="Previous room page" disabled={currentPage === 1} onClick={() => onChange(currentPage - 1)} className="inline-flex size-10 items-center justify-center rounded-md border text-primary-900 disabled:cursor-not-allowed disabled:opacity-40"><ChevronLeft size={18} /></button>{Array.from({ length: totalPages }, (_, index) => index + 1).map((page) => <button key={page} type="button" aria-current={page === currentPage ? "page" : undefined} onClick={() => onChange(page)} className={`inline-flex size-10 items-center justify-center rounded-md border text-sm font-semibold transition-colors ${page === currentPage ? "border-primary bg-primary text-white" : "text-primary-900 hover:bg-gold-100"}`}>{page}</button>)}<button type="button" aria-label="Next room page" disabled={currentPage === totalPages} onClick={() => onChange(currentPage + 1)} className="inline-flex size-10 items-center justify-center rounded-md border text-primary-900 disabled:cursor-not-allowed disabled:opacity-40"><ChevronRight size={18} /></button></nav>
}
