import { Heart, Star } from "lucide-react"
import { Link } from "react-router-dom"

interface HotelCardProps {
    hotelId: string
    name: string
    location: string
    pricePerNight: number
    rating: number
    imageUrl: string
}

export function HotelCard({
    hotelId,
    name,
    location,
    pricePerNight,
    imageUrl,
    rating,
}: HotelCardProps) {
    return (
        <article className="group relative w-full overflow-hidden rounded-lg border bg-card shadow-sm transition-all duration-200 hover:-translate-y-1 hover:shadow-md">
            <Link to={`/hotels/${hotelId}`} className="block">
                <div className="aspect-[4/3] overflow-hidden bg-muted">
                    <img
                        src={imageUrl}
                        alt={name}
                        className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-[1.03]"
                    />
                </div>

                <div className="p-4">
                    <div className="flex items-start justify-between gap-3">
                        <h3 className="line-clamp-1 font-heading text-lg font-semibold leading-[1.4] text-primary-900">
                            {name}
                        </h3>
                        <span className="flex shrink-0 items-center gap-1 text-sm font-medium text-text-secondary">
                            <Star size={16} strokeWidth={1.75} className="fill-gold-500 text-gold-500" />
                            {rating}
                        </span>
                    </div>

                    <p className="mt-1 text-sm text-text-secondary">{location}</p>
                    <p className="mt-4 text-sm text-text-secondary">
                        <span className="text-xl font-bold text-primary-900">${pricePerNight}</span>
                        <span> / night</span>
                    </p>
                </div>
            </Link>

            <button
                type="button"
                aria-label={`Add ${name} to favorites`}
                className="absolute right-3 top-3 inline-flex size-9 items-center justify-center rounded-full bg-white/90 text-primary-800 shadow-sm backdrop-blur transition-colors hover:bg-white hover:text-gold-600"
            >
                <Heart size={18} strokeWidth={1.75} />
            </button>
        </article>
    )
}
