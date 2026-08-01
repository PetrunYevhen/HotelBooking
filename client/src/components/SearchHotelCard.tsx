import { Heart, MapPin, Star } from "lucide-react"
import { Link } from "react-router-dom"

interface SearchHotelCardProps {
    hotelId: string
    name: string
    location: string
    pricePerNight: number
    currency: string
    rating: number
    imageUrl: string
    search: string
    availabilityChecked: boolean
}

function formatPrice(amount: number, currency: string) {
    try {
        return new Intl.NumberFormat("en-US", {
            style: "currency",
            currency,
            maximumFractionDigits: 0,
        }).format(amount)
    } catch {
        return `$${amount}`
    }
}

export function SearchHotelCard({
    hotelId,
    name,
    location,
    pricePerNight,
    currency,
    rating,
    imageUrl,
    search,
    availabilityChecked,
}: SearchHotelCardProps) {
    return (
        <article className="group relative overflow-hidden rounded-xl border bg-white shadow-sm transition-shadow hover:shadow-md md:grid md:grid-cols-[260px_1fr]">
            <Link
                to={`/hotels/${hotelId}${search}`}
                className="contents"
                aria-label={`View ${name}`}
            >
                <div className="aspect-[4/3] overflow-hidden bg-muted md:aspect-auto md:min-h-[220px]">
                    <img
                        src={imageUrl}
                        alt={name}
                        className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-[1.03]"
                    />
                </div>

                <div className="flex min-w-0 flex-col p-5 md:p-6">
                    <div className="flex items-start justify-between gap-4 pr-10">
                        <div className="min-w-0">
                            <p className="flex items-center gap-1.5 text-sm text-text-secondary">
                                <MapPin size={15} className="shrink-0 text-gold-600" />
                                <span className="truncate">{location}</span>
                            </p>
                            <h2 className="mt-2 font-heading text-2xl font-semibold text-primary-900">
                                {name}
                            </h2>
                        </div>

                        <span className="flex shrink-0 items-center gap-1 rounded-full bg-gold-100 px-2.5 py-1 text-sm font-semibold text-primary-900">
                            <Star
                                size={15}
                                className="fill-gold-500 text-gold-500"
                            />
                            {rating > 0 ? rating.toFixed(1) : "New"}
                        </span>
                    </div>

                    <p className="mt-4 text-sm leading-6 text-text-secondary">
                        Comfortable stay with flexible booking options and carefully selected rooms.
                    </p>

                    <div className="mt-auto flex items-end justify-between gap-4 pt-5">
                        <span className="text-xs font-medium text-success-600">
                            {availabilityChecked
                                ? "Available for your dates"
                                : "Rooms matching your search"}
                        </span>

                        <p className="shrink-0 text-right text-xs text-text-muted">
                            From
                            <span className="ml-1 text-xl font-bold text-primary-900">
                                {formatPrice(pricePerNight, currency)}
                            </span>
                            <span className="block">per night</span>
                        </p>
                    </div>
                </div>
            </Link>

            <button
                type="button"
                aria-label={`Add ${name} to favorites`}
                className="absolute right-4 top-4 inline-flex size-9 items-center justify-center rounded-full bg-white/90 text-primary-800 shadow-sm transition-colors hover:text-gold-600 md:top-5"
            >
                <Heart size={18} />
            </button>
        </article>
    )
}
