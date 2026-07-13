import { useEffect, useState } from "react"
import { ArrowRight } from "lucide-react"
import { HotelCard } from "../components/HotelCard"
import { getAllHotels, type HotelDto } from "../api/hotels"

export function Home() {
    const [hotels, setHotels] = useState<HotelDto[]>([])
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        getAllHotels()
            .then(setHotels)
            .finally(() => setLoading(false))
    }, [])

    return (
        <div className="flex flex-col gap-8">
            <div className="flex items-center justify-between bg-blue-50 rounded-xl p-4">
                <p className="text-sm">You Can Change Your Location to show nearby villas</p>
                <ArrowRight size={18} />
            </div>

            <section>
                <h2 className="font-semibold mb-3">All Hotels</h2>

                {loading && <p className="text-sm text-gray-500">Loading...</p>}
                {!loading && hotels.length === 0 && <p className="text-sm text-gray-500">No hotels found.</p>}

                <div className="flex gap-4 flex-wrap">
                    {hotels.map((hotel) => (
                        <HotelCard
                            key={hotel.hotelId}
                            hotelId={hotel.hotelId}
                            name={hotel.name}
                            location={`${hotel.city}, ${hotel.country}`}
                            pricePerNight={hotel.minRoomPriceAmount ?? 0}
                            rating={hotel.rating ?? 0}
                            imageUrl="https://placehold.co/400x300"
                        />
                    ))}
                </div>
            </section>
        </div>
    )
}