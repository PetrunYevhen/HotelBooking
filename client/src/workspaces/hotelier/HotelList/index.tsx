import { useEffect, useState } from "react"
import { Link } from "react-router-dom"
import { getHotelierHotels, type HotelierHotel } from "@/domains/accommodations/api/hotelier"
import { ApiError } from "@/shared/lib/api-client"

export function HotelierHotelList() {
    const [hotels, setHotels] = useState<HotelierHotel[]>([])
    const [error, setError] = useState("")
    const load = async () => {
        try {
            const data = await getHotelierHotels()
            setHotels(data)
        } catch (cause) {
            setError(cause instanceof ApiError ? cause.message : "Could not load hotels.")
        }
    }

    useEffect(() => { void load() }, [])

    return <main className="stayora-container stayora-page"><p className="section-eyebrow">Hotelier workspace</p><h1 className="section-title mt-1">Your properties</h1>{error && <p role="alert" className="mt-4 text-sm text-error-600">{error}</p>}<div className="mt-6 grid gap-4 md:grid-cols-2">{hotels.map(hotel => <article key={hotel.hotelId} className="stayora-card"><h2 className="font-heading text-xl font-semibold text-primary-900">{hotel.name}</h2><p className="text-sm text-text-secondary">{hotel.city}, {hotel.country}</p><Link to={`/hotelier/hotels/${hotel.hotelId}/overview`} className="mt-4 inline-block text-sm font-semibold text-primary-700">Open workspace →</Link></article>)}{!hotels.length && !error && <p className="text-sm text-text-muted">No assigned properties yet.</p>}</div></main>
}
