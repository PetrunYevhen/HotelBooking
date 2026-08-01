import { useEffect, useMemo, useState } from "react"
import { Heart, MapPin, Share2, Star } from "lucide-react"
import { Link, useNavigate, useParams, useSearchParams } from "react-router-dom"
import { getHotelById, getHotelFacilities, type FacilityDto, type HotelDetailsDto } from "@/api/hotels"
import { getRoomsByHotelId, type RoomDetailsDto } from "@/api/rooms"
import { roomImages } from "./constants"
import { RoomBookingCard } from "./RoomBookingCard"
import { RoomFeatures, RoomAmenities, RoomPolicies } from "./RoomInformation"
import { RoomGallery } from "./RoomGallery"
import { addDays, calculateNights, toLocalDateValue } from "@/pages/HotelDetails/utils"

export function RoomDetails() {
    const { hotelId, roomId } = useParams<{ hotelId: string; roomId: string }>()
    const navigate = useNavigate()
    const [searchParams] = useSearchParams()
    const today = toLocalDateValue(new Date())
    const checkIn = searchParams.get("checkIn") || today
    const checkOut = searchParams.get("checkOut") || addDays(checkIn, 1)
    const guests = Math.max(1, Number(searchParams.get("guests") ?? 1))
    const [hotel, setHotel] = useState<HotelDetailsDto | null>(null)
    const [room, setRoom] = useState<RoomDetailsDto | null>(null)
    const [facilities, setFacilities] = useState<FacilityDto[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [isSaved, setIsSaved] = useState(false)
    const [shareLabel, setShareLabel] = useState("Share")
    const [selectedImage, setSelectedImage] = useState<string | null>(null)

    useEffect(() => {
        if (!hotelId || !roomId) { setError("Room was not found."); setIsLoading(false); return }
        let isCurrent = true
        setIsLoading(true); setError(null)
        Promise.all([getHotelById(hotelId), getHotelFacilities(hotelId), getRoomsByHotelId(hotelId, checkIn)])
            .then(([hotelResult, facilityResult, rooms]) => { if (!isCurrent) return; setHotel(hotelResult); setFacilities(facilityResult); setRoom(rooms.find((item) => item.roomId === roomId) ?? null) })
            .catch(() => { if (isCurrent) setError("Could not load this room.") })
            .finally(() => { if (isCurrent) setIsLoading(false) })
        return () => { isCurrent = false }
    }, [hotelId, roomId, checkIn])

    const nights = calculateNights(checkIn, checkOut)
    const total = room ? room.effectivePriceAmount * nights : 0
    const returnToHotel = hotelId ? `/hotels/${hotelId}${searchParams.toString() ? `?${searchParams.toString()}` : ""}` : "/search"
    const roomDescription = useMemo(() => room?.description || "A comfortable, thoughtfully appointed room designed for a restful stay.", [room])

    async function shareRoom() {
        try { if (navigator.share) { await navigator.share({ title: room?.type, url: window.location.href }); return } await navigator.clipboard.writeText(window.location.href); setShareLabel("Link copied") } catch { setShareLabel("Share failed") }
        window.setTimeout(() => setShareLabel("Share"), 2_000)
    }

    function openBooking() {
        if (!hotelId || !roomId) return
        const checkoutParams = new URLSearchParams({ hotelId, roomId, checkIn, checkOut, guests: String(Math.min(guests, room?.capacity ?? guests)) })
        navigate(`/checkout?${checkoutParams.toString()}`)
    }
    if (isLoading) return <RoomDetailsSkeleton />
    if (error || !hotel || !room) return <RoomUnavailable error={error} returnToHotel={returnToHotel} />

    return <div className="stayora-container py-8 md:py-12"><nav className="flex flex-wrap items-center gap-2 text-sm text-text-muted" aria-label="Breadcrumb"><Link to="/" className="hover:text-primary-800">Home</Link><span>›</span><Link to={returnToHotel} className="hover:text-primary-800">{hotel.name}</Link><span>›</span><span className="text-text-secondary">{room.type}</span></nav><div className="mt-7 flex flex-col justify-between gap-5 md:flex-row md:items-end"><div><h1 className="font-heading text-4xl font-semibold leading-tight text-primary-900 md:text-5xl">{room.type}</h1><div className="mt-3 flex flex-wrap items-center gap-x-4 gap-y-2 text-sm text-text-secondary"><span className="flex items-center gap-1 text-primary-900"><Star size={15} className="fill-gold-500 text-gold-500" />{hotel.rating?.toFixed(1) ?? "New"}</span><span>{hotel.name}</span><span className="flex items-center gap-1"><MapPin size={15} className="text-gold-600" />{hotel.city}, {hotel.country}</span></div></div><div className="flex gap-2"><button type="button" onClick={() => void shareRoom()} className="inline-flex min-h-11 items-center gap-2 rounded-md border bg-white px-4 text-sm font-semibold text-primary-800 hover:bg-muted"><Share2 size={17} />{shareLabel}</button><button type="button" onClick={() => setIsSaved((saved) => !saved)} aria-pressed={isSaved} className="inline-flex min-h-11 items-center gap-2 rounded-md border bg-white px-4 text-sm font-semibold text-primary-800 hover:bg-muted"><Heart size={17} className={isSaved ? "fill-gold-500 text-gold-500" : ""} />{isSaved ? "Saved" : "Save"}</button></div></div><div className="relative mt-7"><div className="lg:mr-[400px]"><RoomGallery roomName={room.type} images={roomImages} onOpen={setSelectedImage} /></div><RoomBookingCard room={room} image={roomImages[0]} checkIn={checkIn} checkOut={checkOut} guests={Math.min(guests, room.capacity)} nights={nights} total={total} onBook={openBooking} /><main className="mt-7 lg:mr-[400px]"><RoomFeatures room={room} /><p className="mt-7 text-base leading-7 text-text-secondary">{roomDescription}</p><RoomAmenities facilities={facilities} /><RoomPolicies /><section className="mt-9 rounded-xl bg-bg-warm p-6"><p className="section-eyebrow">Guest reviews</p><h2 className="section-title mt-1">A stay guests remember</h2><div className="mt-4 flex items-center gap-4"><span className="font-heading text-4xl font-semibold text-primary-900">{hotel.rating?.toFixed(1) ?? "New"}</span><div><div className="flex text-gold-500">{[0, 1, 2, 3, 4].map((star) => <Star key={star} size={16} className={hotel.rating && star < Math.round(hotel.rating) ? "fill-current" : ""} />)}</div><p className="mt-1 text-xs text-text-muted">Verified guest rating</p></div></div></section></main></div>{selectedImage && <ImageDialog roomName={room.type} image={selectedImage} onClose={() => setSelectedImage(null)} />}</div>
}

function ImageDialog({ roomName, image, onClose }: { roomName: string; image: string; onClose: () => void }) { return <div role="dialog" aria-modal="true" aria-label="Room image" className="fixed inset-0 z-[70] flex items-center justify-center bg-primary-900/95 p-4" onClick={onClose}><button type="button" onClick={onClose} className="absolute right-5 top-5 rounded-full bg-white/10 px-4 py-2 text-sm font-semibold text-white">Close</button><img src={image} alt={`${roomName} full view`} className="max-h-[85vh] max-w-[95vw] rounded-xl object-contain" onClick={(event) => event.stopPropagation()} /></div> }
function RoomUnavailable({ error, returnToHotel }: { error: string | null; returnToHotel: string }) { return <div className="stayora-container py-20 text-center"><h1 className="font-heading text-3xl font-semibold text-primary-900">Room unavailable</h1><p className="mt-3 text-text-secondary">{error || "This room could not be found for the selected dates."}</p><Link to={returnToHotel} className="mt-6 inline-flex min-h-11 items-center rounded-md bg-primary px-5 text-sm font-semibold text-white">Back to hotel</Link></div> }
function RoomDetailsSkeleton() { return <div className="stayora-container animate-pulse py-12"><div className="h-4 w-40 rounded bg-muted" /><div className="mt-6 h-12 w-2/3 rounded bg-muted" /><div className="mt-7 h-[460px] rounded-xl bg-muted lg:mr-[400px]" /><div className="mt-7 h-32 rounded-xl bg-muted lg:mr-[400px]" /></div> }
