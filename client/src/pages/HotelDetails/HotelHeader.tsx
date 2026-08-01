import { Heart, MapPin, Share2, Star } from "lucide-react"
import { Link } from "react-router-dom"
import type { HotelDetailsDto } from "@/api/hotels"

interface HotelHeaderProps {
    hotel: HotelDetailsDto
    returnSearch: string
    isSaved: boolean
    shareLabel: string
    onSave: () => void
    onShare: () => void
}

export function HotelHeader({ hotel, returnSearch, isSaved, shareLabel, onSave, onShare }: HotelHeaderProps) {
    return (
        <>
            <Link to={returnSearch} className="inline-flex items-center gap-1 text-sm font-medium text-text-secondary transition-colors hover:text-primary-900">
                ← Back to search results
            </Link>
            <header className="mt-5 flex flex-col justify-between gap-5 md:flex-row md:items-end">
                <div>
                    <div className="flex items-center gap-2">
                        <span className="flex items-center gap-1 rounded-full bg-gold-100 px-2.5 py-1 text-sm font-semibold text-primary-900">
                            <Star size={15} className="fill-gold-500 text-gold-500" /> {hotel.rating?.toFixed(1) ?? "New"}
                        </span>
                        <span className="text-sm text-text-muted">Verified property</span>
                    </div>
                    <h1 className="mt-3 font-heading text-4xl font-semibold leading-tight text-primary-900 md:text-5xl">{hotel.name}</h1>
                    <p className="mt-3 flex items-start gap-2 text-sm text-text-secondary"><MapPin size={17} className="mt-0.5 shrink-0 text-gold-600" />{hotel.street}, {hotel.city}, {hotel.postalCode}, {hotel.country}</p>
                </div>
                <div className="flex gap-2">
                    <button type="button" onClick={onShare} className="inline-flex min-h-11 items-center gap-2 rounded-md border bg-white px-4 text-sm font-semibold text-primary-800 hover:bg-muted"><Share2 size={17} /> {shareLabel}</button>
                    <button type="button" onClick={onSave} aria-pressed={isSaved} className="inline-flex min-h-11 items-center gap-2 rounded-md border bg-white px-4 text-sm font-semibold text-primary-800 hover:bg-muted"><Heart size={17} className={isSaved ? "fill-gold-500 text-gold-500" : ""} />{isSaved ? "Saved" : "Save"}</button>
                </div>
            </header>
        </>
    )
}
