import { SearchX } from "lucide-react"
import type { HotelDto } from "@/api/hotels"
import { SearchHotelCard } from "@/components/SearchHotelCard"
import { hotelImages } from "./data"

interface ResultsContentProps { hotels: HotelDto[]; isLoading: boolean; error: string | null; search: string; availabilityChecked: boolean; onReset: () => void }

export function ResultsContent({ hotels, isLoading, error, search, availabilityChecked, onReset }: ResultsContentProps) {
    if (isLoading) return <ResultsSkeleton />
    if (error) return <div role="alert" className="rounded-xl border border-error-100 bg-error-50 p-6"><p className="font-semibold text-error-600">{error}</p><p className="mt-1 text-sm text-text-secondary">Check that the API is running and try the search again.</p></div>
    if (!hotels.length) return <div className="rounded-xl border bg-white px-6 py-14 text-center shadow-sm"><SearchX size={40} className="mx-auto text-gold-500" /><h2 className="mt-4 font-heading text-2xl font-semibold text-primary-900">No matching stays found</h2><p className="mx-auto mt-2 max-w-md text-sm text-text-secondary">Try increasing the price limit, lowering the rating, or changing your search dates.</p><button type="button" onClick={onReset} className="mt-5 text-sm font-semibold text-primary-700 hover:text-primary-900">Reset filters</button></div>
    return <div className="space-y-5">{hotels.map((hotel, index) => <SearchHotelCard key={hotel.hotelId} hotelId={hotel.hotelId} name={hotel.name} location={`${hotel.city}, ${hotel.country}`} pricePerNight={hotel.minRoomPriceAmount ?? 0} currency={hotel.minRoomPriceCurrency || "USD"} rating={hotel.rating ?? 0} imageUrl={hotelImages[index % hotelImages.length]} search={search} availabilityChecked={availabilityChecked} />)}</div>
}

function ResultsSkeleton() {
    return <div className="space-y-5" aria-label="Loading hotels">{Array.from({ length: 3 }, (_, index) => <div key={index} className="overflow-hidden rounded-xl border bg-white md:grid md:grid-cols-[260px_1fr]"><div className="aspect-[4/3] animate-pulse bg-muted md:aspect-auto md:min-h-[220px]" /><div className="space-y-4 p-6"><div className="h-4 w-1/3 animate-pulse rounded bg-muted" /><div className="h-7 w-2/3 animate-pulse rounded bg-muted" /><div className="h-4 w-full animate-pulse rounded bg-muted" /><div className="h-4 w-3/4 animate-pulse rounded bg-muted" /></div></div>)}</div>
}
