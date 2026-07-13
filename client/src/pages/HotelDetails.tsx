import { useParams } from "react-router-dom"
import { useEffect, useState } from "react"
import { Star, MapPin } from "lucide-react"
import { getHotelById, getHotelFacilities, type HotelDetailsDto, type FacilityDto } from "../api/hotels"
import { getRoomsByHotelId, type RoomDetailsDto } from "../api/rooms"
import {RoomCard} from "@/components/RoomCard.tsx";

export function HotelDetails() {
  const { id: hotelId } = useParams<{ id: string }>()

  const [hotel, setHotel] = useState<HotelDetailsDto | null>(null)
  const [facilities, setFacilities] = useState<FacilityDto[]>([])
  const [rooms, setRooms] = useState<RoomDetailsDto[]>([])
  const [checkIn, setCheckIn] = useState(() => new Date().toISOString().split("T")[0])
  const [checkOut, setCheckOut] = useState(() => {
    const tomorrow = new Date()
    tomorrow.setDate(tomorrow.getDate() + 1)
    return tomorrow.toISOString().split("T")[0]
  })

  useEffect(() => {
    if (!hotelId) return
    getHotelById(hotelId).then(setHotel)
  }, [hotelId])

  useEffect(() => {
    if (!hotelId) return
    getHotelFacilities(hotelId).then(setFacilities)
  }, [hotelId])

  useEffect(() => {
    if (!hotelId) return
    getRoomsByHotelId(hotelId, checkIn).then(setRooms)
  }, [hotelId, checkIn])

  if (!hotel) return <p>Loading...</p>

  return (
    <div className="flex flex-col gap-8">
      <section>
        <h1 className="text-2xl font-semibold">{hotel.name}</h1>
        <div className="flex items-center gap-4 text-sm text-gray-500 mt-1">
          <span className="flex items-center gap-1">
            <Star size={14} className="fill-yellow-400 text-yellow-400" />
            {hotel.rating ?? "—"}
          </span>
          <span className="flex items-center gap-1">
            <MapPin size={14} />
            {hotel.city}, {hotel.country}
          </span>
        </div>
        <p className="mt-4 text-sm text-gray-600">{hotel.description}</p>
      </section>

      <section>
        <h2 className="font-semibold mb-3">Facilities</h2>
        <div className="flex flex-col gap-4">
          {Object.entries(
              facilities.reduce<Record<string, typeof facilities>>((groups, f) => {
                (groups[f.category] ??= []).push(f)
                return groups
              }, {})
          ).map(([category, items]) => (
              <div key={category}>
                <h3 className="text-sm font-medium text-gray-500 mb-2">{category}</h3>
                <div className="flex gap-2 flex-wrap">
                  {items.map((f) => (
                      <span key={f.name} className="px-3 py-1 rounded-full bg-gray-100 text-sm">
              {f.name}
            </span>
                  ))}
                </div>
              </div>
          ))}
        </div>
      </section>

      <section>
        <h2 className="font-semibold mb-3">Select Dates</h2>
        <div className="flex gap-4">
          <label className="text-sm text-gray-500 flex flex-col gap-1">
            Check-in
            <input
              type="date"
              value={checkIn}
              onChange={(e) => setCheckIn(e.target.value)}
              className="border rounded-lg px-3 py-2 text-sm"
            />
          </label>
          <label className="text-sm text-gray-500 flex flex-col gap-1">
            Check-out
            <input
              type="date"
              value={checkOut}
              min={checkIn}
              onChange={(e) => setCheckOut(e.target.value)}
              className="border rounded-lg px-3 py-2 text-sm"
            />
          </label>
        </div>
      </section>

      <section>
        <h2 className="font-semibold mb-3">Rooms</h2>
        <div className="flex flex-col gap-3">
          {rooms.map((room) => (
              <RoomCard key={room.roomId} room={room} checkIn={checkIn} checkOut={checkOut} />
          ))}
        </div>
      </section>
    </div>
  )
}
