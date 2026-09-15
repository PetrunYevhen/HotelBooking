import { MapPin } from "lucide-react"

interface DestinationCardProps {
    name: string
    country: string
    imageUrl: string
    priceFrom: number
}

export function DestinationCard({
    name,
    country,
    imageUrl,
    priceFrom,
}: DestinationCardProps) {
    return (
        <a
            href="#hotels"
            className="group relative block aspect-[4/3] overflow-hidden rounded-lg bg-primary-900 shadow-sm"
            aria-label={`Explore hotels in ${name}, ${country}`}
        >
            <img
                src={imageUrl}
                alt={`${name}, ${country}`}
                className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-105"
            />

            <div
                aria-hidden="true"
                className="absolute inset-0 bg-gradient-to-t from-primary-900/90 via-primary-900/10 to-transparent"
            />

            <span className="absolute right-3 top-3 inline-flex size-8 items-center justify-center rounded-full bg-white/90 text-primary-800 shadow-sm backdrop-blur">
                <MapPin size={16} strokeWidth={1.75} />
            </span>

            <div className="absolute inset-x-0 bottom-0 p-4 text-white">
                <h3 className="font-heading text-lg font-semibold text-white">
                    {name}
                </h3>
                <p className="mt-0.5 text-xs text-white/80">{country}</p>
                <p className="mt-2 text-xs font-medium text-white/90">
                    Stays from ${priceFrom}
                </p>
            </div>
        </a>
    )
}
