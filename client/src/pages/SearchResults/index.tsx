import { useEffect, useMemo, useState } from "react"
import { Link, useSearchParams } from "react-router-dom"
import { searchHotels, type HotelDto } from "@/api/hotels"
import { SearchFilters } from "./Filters"
import { ResultsContent } from "./ResultsContent"
import { SearchSummary } from "./SearchSummary"
import type { SortOption } from "./types"

export function SearchResults() {
    const [searchParams] = useSearchParams()
    const [hotels, setHotels] = useState<HotelDto[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [sort, setSort] = useState<SortOption>("recommended")
    const [maximumPrice, setMaximumPrice] = useState(500)
    const [minimumRating, setMinimumRating] = useState(0)
    const destination = searchParams.get("destination") ?? undefined
    const checkIn = searchParams.get("checkIn") ?? undefined
    const checkOut = searchParams.get("checkOut") ?? undefined
    const guests = Number(searchParams.get("guests") ?? 1)
    const rooms = Number(searchParams.get("rooms") ?? 1)
    const children = Number(searchParams.get("children") ?? 0)
    const queryString = searchParams.toString()
    const hotelDetailsSearch = queryString ? `?${queryString}` : ""

    useEffect(() => { let isCurrent = true; setIsLoading(true); setError(null); searchHotels({ destination, checkIn, checkOut, guests, rooms }).then((result) => { if (isCurrent) setHotels(result) }).catch(() => { if (isCurrent) setError("Could not load search results.") }).finally(() => { if (isCurrent) setIsLoading(false) }); return () => { isCurrent = false } }, [destination, checkIn, checkOut, guests, rooms])
    const visibleHotels = useMemo(() => hotels.filter((hotel) => (hotel.minRoomPriceAmount ?? 0) <= maximumPrice && (hotel.rating ?? 0) >= minimumRating).toSorted((first, second) => sort === "rating" ? (second.rating ?? 0) - (first.rating ?? 0) : sort === "price-low" ? (first.minRoomPriceAmount ?? 0) - (second.minRoomPriceAmount ?? 0) : sort === "price-high" ? (second.minRoomPriceAmount ?? 0) - (first.minRoomPriceAmount ?? 0) : 0), [hotels, maximumPrice, minimumRating, sort])
    function resetFilters() { setMaximumPrice(500); setMinimumRating(0); setSort("recommended") }
    return <div className="stayora-container py-8 md:py-12"><nav className="mb-5 text-sm text-text-muted" aria-label="Breadcrumb"><Link to="/" className="hover:text-primary-800">Home</Link><span aria-hidden="true"> / </span><span className="text-text-secondary">Search results</span></nav><SearchSummary destination={destination} checkIn={checkIn} checkOut={checkOut} guests={guests} rooms={rooms} childrenCount={children} /><div className="mt-9 flex flex-wrap items-end justify-between gap-4"><div><p className="section-eyebrow">Places to stay</p><h1 className="mt-1 font-heading text-3xl font-semibold text-primary-900 md:text-4xl">{destination ? `Hotels in ${destination}` : "Available hotels"}</h1><p className="mt-2 text-sm text-text-secondary">{isLoading ? "Searching for the best stays…" : `${visibleHotels.length} ${visibleHotels.length === 1 ? "property" : "properties"} found`}</p></div><label className="flex items-center gap-3 text-sm text-text-secondary">Sort by<select value={sort} onChange={(event) => setSort(event.target.value as SortOption)} className="min-h-11 rounded-md border bg-white px-3 text-sm font-medium text-primary-900 outline-none focus:border-gold-500 focus:ring-3 focus:ring-gold-500/15"><option value="recommended">Recommended</option><option value="rating">Highest rating</option><option value="price-low">Price: low to high</option><option value="price-high">Price: high to low</option></select></label></div><div className="mt-7 grid gap-7 lg:grid-cols-[260px_minmax(0,1fr)]"><aside><SearchFilters maximumPrice={maximumPrice} minimumRating={minimumRating} onMaximumPriceChange={setMaximumPrice} onMinimumRatingChange={setMinimumRating} onReset={resetFilters} /></aside><main><ResultsContent hotels={visibleHotels} isLoading={isLoading} error={error} search={hotelDetailsSearch} availabilityChecked={Boolean(checkIn && checkOut)} onReset={resetFilters} /></main></div></div>
}
