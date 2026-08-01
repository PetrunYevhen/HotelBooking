import { useEffect, useMemo, useState } from "react"
import { Link, useNavigate, useParams, useSearchParams } from "react-router-dom"
import { getHotelById, getHotelFacilities, type FacilityDto, type HotelDetailsDto } from "@/api/hotels"
import { getRoomsByHotelId, type RoomDetailsDto } from "@/api/rooms"
import { AmenitiesSection } from "./AmenitiesSection"
import { BookingSidebar } from "./BookingSidebar"
import { fallbackAmenities, hotelImages } from "./constants"
import { HotelGallery } from "./HotelGallery"
import { HotelHeader } from "./HotelHeader"
import { HotelNavigation } from "./HotelNavigation"
import { OverviewSection } from "./OverviewSection"
import { ReviewsSection, PoliciesSection } from "./ReviewsAndPolicies"
import { RoomsSection } from "./RoomsSection"
import { addDays, calculateNights, toLocalDateValue } from "./utils"

export function HotelDetails() {
    const { id: hotelId } = useParams<{ id: string }>()
    const navigate = useNavigate()
    const [searchParams] = useSearchParams()
    const today = toLocalDateValue(new Date())
    const initialCheckIn = searchParams.get("checkIn") || today
    const initialCheckOut = searchParams.get("checkOut") || addDays(initialCheckIn, 1)
    const guests = Math.max(1, Number(searchParams.get("guests") ?? 1))
    const roomsRequested = Math.max(1, Number(searchParams.get("rooms") ?? 1))
    const [hotel, setHotel] = useState<HotelDetailsDto | null>(null)
    const [facilities, setFacilities] = useState<FacilityDto[]>([])
    const [rooms, setRooms] = useState<RoomDetailsDto[]>([])
    const [checkIn, setCheckIn] = useState(initialCheckIn)
    const [checkOut, setCheckOut] = useState(initialCheckOut)
    const [isHotelLoading, setIsHotelLoading] = useState(true)
    const [isRoomsLoading, setIsRoomsLoading] = useState(true)
    const [pageError, setPageError] = useState<string | null>(null)
    const [roomsError, setRoomsError] = useState<string | null>(null)
    const [selectedImage, setSelectedImage] = useState<string | null>(null)
    const [isSaved, setIsSaved] = useState(false)
    const [shareLabel, setShareLabel] = useState("Share")
    const [selectedRoomId, setSelectedRoomId] = useState<string | null>(null)
    const [showAllRooms, setShowAllRooms] = useState(false)
    const [roomPage, setRoomPage] = useState(1)
    const [activeTab, setActiveTab] = useState("overview")

    useEffect(() => {
        if (!hotelId) {
            setPageError("Hotel was not found.")
            setIsHotelLoading(false)
            return
        }
        let isCurrent = true
        setIsHotelLoading(true)
        setPageError(null)
        Promise.all([getHotelById(hotelId), getHotelFacilities(hotelId)])
            .then(([hotelResult, facilityResult]) => { if (isCurrent) { setHotel(hotelResult); setFacilities(facilityResult) } })
            .catch(() => { if (isCurrent) setPageError("Could not load this hotel.") })
            .finally(() => { if (isCurrent) setIsHotelLoading(false) })
        return () => { isCurrent = false }
    }, [hotelId])

    useEffect(() => {
        if (!hotelId) return
        let isCurrent = true
        setIsRoomsLoading(true)
        setRoomsError(null)
        getRoomsByHotelId(hotelId, checkIn)
            .then((result) => { if (isCurrent) setRooms(result) })
            .catch(() => { if (isCurrent) setRoomsError("Could not load available rooms.") })
            .finally(() => { if (isCurrent) setIsRoomsLoading(false) })
        return () => { isCurrent = false }
    }, [hotelId, checkIn])

    const guestsPerRoom = Math.ceil(guests / roomsRequested)
    const matchingRooms = useMemo(() => rooms.filter((room) => room.isActive && room.capacity >= guestsPerRoom), [rooms, guestsPerRoom])
    const displayedAmenities = facilities.length > 0 ? facilities.slice(0, 8) : fallbackAmenities
    const roomsPerPage = 10
    const totalRoomPages = Math.max(1, Math.ceil(matchingRooms.length / roomsPerPage))
    const currentRoomPage = Math.min(roomPage, totalRoomPages)
    const visibleRooms = showAllRooms ? matchingRooms.slice((currentRoomPage - 1) * roomsPerPage, currentRoomPage * roomsPerPage) : matchingRooms.slice(0, 3)
    const selectedRoom = matchingRooms.find((room) => room.roomId === selectedRoomId) ?? null
    const nights = calculateNights(checkIn, checkOut)
    const roomSubtotal = selectedRoom ? selectedRoom.effectivePriceAmount * nights * roomsRequested : 0
    const returnSearch = searchParams.toString() ? `/search?${searchParams.toString()}` : "/search"
    const highlights = facilities.length > 0 ? facilities.slice(0, 4).map((item) => item.name) : ["Comfortable rooms", "Great location", "Secure booking"]

    function handleCheckInChange(value: string) {
        setCheckIn(value)
        if (checkOut <= value) setCheckOut(addDays(value, 1))
    }

    async function handleShare() {
        try {
            if (navigator.share) { await navigator.share({ title: hotel?.name, url: window.location.href }); return }
            await navigator.clipboard.writeText(window.location.href)
            setShareLabel("Link copied")
        } catch { setShareLabel("Share failed") }
        window.setTimeout(() => setShareLabel("Share"), 2_000)
    }

    function selectRoom(room: RoomDetailsDto) {
        setSelectedRoomId(room.roomId)
    }

    function openAllRooms() { setShowAllRooms(true); setRoomPage(1); setActiveTab("rooms") }
    function handleTabNavigation(target: string) {
        setActiveTab(target)
        if (target === "rooms") openAllRooms()
        if (target === "overview") setShowAllRooms(false)
    }
    function reserveSelectedRoom() {
        if (!selectedRoom) return
        const checkoutParams = new URLSearchParams({ hotelId: selectedRoom.hotelId, roomId: selectedRoom.roomId, checkIn, checkOut, guests: String(Math.min(guestsPerRoom, selectedRoom.capacity)) })
        navigate(`/checkout?${checkoutParams.toString()}`)
    }

    if (isHotelLoading) return <HotelDetailsSkeleton />
    if (pageError || !hotel) return <HotelUnavailable error={pageError} returnSearch={returnSearch} />

    return (
        <div className="stayora-container py-8 md:py-12">
            <HotelHeader hotel={hotel} returnSearch={returnSearch} isSaved={isSaved} shareLabel={shareLabel} onSave={() => setIsSaved((current) => !current)} onShare={() => void handleShare()} />
            <div className="relative mt-7">
                <div className="lg:mr-[400px]"><HotelGallery hotelName={hotel.name} images={hotelImages} onOpen={setSelectedImage} /></div>
                <BookingSidebar hotel={hotel} today={today} checkIn={checkIn} checkOut={checkOut} guests={guests} roomsRequested={roomsRequested} nights={nights} selectedRoom={selectedRoom} roomSubtotal={roomSubtotal} onCheckInChange={handleCheckInChange} onCheckOutChange={setCheckOut} onReserve={reserveSelectedRoom} />
                <main className="mt-8 lg:mr-[400px]">
                    <HotelNavigation activeTab={activeTab} onNavigate={handleTabNavigation} />
                    <OverviewSection hotel={hotel} highlights={highlights} />
                    <AmenitiesSection amenities={displayedAmenities} />
                    <RoomsSection rooms={visibleRooms} isLoading={isRoomsLoading} error={roomsError} checkIn={checkIn} checkOut={checkOut} nights={nights} guestsPerRoom={guestsPerRoom} selectedRoomId={selectedRoomId} showAll={showAllRooms} currentPage={currentRoomPage} totalPages={totalRoomPages} totalRooms={matchingRooms.length} onSelect={selectRoom} onShowAll={openAllRooms} onPageChange={setRoomPage} />
                    <ReviewsSection rating={hotel.rating} />
                    <PoliciesSection checkIn={checkIn} checkOut={checkOut} />
                </main>
            </div>
            {selectedImage && <ImageDialog hotelName={hotel.name} image={selectedImage} onClose={() => setSelectedImage(null)} />}
        </div>
    )
}

function ImageDialog({ hotelName, image, onClose }: { hotelName: string; image: string; onClose: () => void }) {
    return <div role="dialog" aria-modal="true" aria-label="Hotel image" className="fixed inset-0 z-[70] flex items-center justify-center bg-primary-900/95 p-4" onClick={onClose}><button type="button" onClick={onClose} className="absolute right-5 top-5 rounded-full bg-white/10 px-4 py-2 text-sm font-semibold text-white">Close</button><img src={image} alt={`${hotelName} full view`} className="max-h-[85vh] max-w-[95vw] rounded-xl object-contain" onClick={(event) => event.stopPropagation()} /></div>
}

function HotelUnavailable({ error, returnSearch }: { error: string | null; returnSearch: string }) {
    return <div className="stayora-container py-20 text-center"><h1 className="font-heading text-3xl font-semibold text-primary-900">Hotel unavailable</h1><p className="mt-3 text-text-secondary">{error || "This hotel could not be found."}</p><Link to={returnSearch} className="mt-6 inline-flex min-h-11 items-center rounded-md bg-primary px-5 text-sm font-semibold text-white">Back to search</Link></div>
}

function HotelDetailsSkeleton() {
    return <div className="stayora-container animate-pulse py-12"><div className="h-4 w-40 rounded bg-muted" /><div className="mt-6 h-12 w-2/3 rounded bg-muted" /><div className="mt-7 h-[460px] rounded-xl bg-muted lg:mr-[400px]" /><div className="mt-8 space-y-4 lg:mr-[400px]"><div className="h-7 w-1/3 rounded bg-muted" /><div className="h-4 w-full rounded bg-muted" /><div className="h-4 w-4/5 rounded bg-muted" /></div></div>
}
