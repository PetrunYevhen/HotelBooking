import { useEffect, useState } from "react"
import { getAllHotels, type HotelDto } from "@/api/hotels"
import { CardSection } from "@/components/CardSection"
import { DestinationCard } from "@/components/DestinationCard"
import { HotelCard } from "@/components/HotelCard"
import { HomeHero } from "@/components/HomeHero"
import { popularDestinations } from "./destinations"

export function Home() {
    const [hotels, setHotels] = useState<HotelDto[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        getAllHotels().then(setHotels).catch(() => setError("Could not load hotels.")).finally(() => setIsLoading(false))
    }, [])

    return <><HomeHero /><div className="stayora-container"><CardSection id="destinations" eyebrow="Explore" title="Popular destinations" actionLabel="View all destinations" actionHref="destinations" columns={5}>{popularDestinations.map((destination) => <DestinationCard key={destination.name} {...destination} />)}</CardSection><CardSection id="hotels" eyebrow="For you" title="Top-rated stays" actionLabel="View all properties" actionHref="#hotels" columns={4}>{isLoading && Array.from({ length: 4 }, (_, index) => <div key={index} className="aspect-[4/3] animate-pulse rounded-lg bg-muted" aria-hidden="true" />)}{hotels.map((hotel) => <HotelCard key={hotel.hotelId} hotelId={hotel.hotelId} name={hotel.name} location={`${hotel.city}, ${hotel.country}`} pricePerNight={hotel.minRoomPriceAmount ?? 0} rating={hotel.rating ?? 0} imageUrl="https://placehold.co/600x450" />)}</CardSection>{error && <p role="alert" className="pb-10 text-sm text-error-600">{error}</p>}</div></>
}
