import { Check } from "lucide-react"
import type { HotelDetailsDto } from "@/api/hotels"

export function OverviewSection({ hotel, highlights }: { hotel: HotelDetailsDto; highlights: string[] }) {
    return (
        <section id="overview" className="scroll-mt-24 pt-7">
            <div className="grid gap-8 md:grid-cols-[1.2fr_1fr]">
                <div>
                    <h2 className="font-heading text-xl font-semibold text-primary-900">About this property</h2>
                    <p className="mt-3 leading-7 text-text-secondary">{hotel.description || "Discover a comfortable stay in a carefully selected location, with rooms designed for a relaxing trip."}</p>
                </div>
                <div>
                    <h2 className="font-heading text-xl font-semibold text-primary-900">Property highlights</h2>
                    <ul className="mt-3 space-y-2.5">
                        {highlights.map((highlight) => (
                            <li key={highlight} className="flex items-center gap-2 text-sm text-text-secondary"><Check size={16} className="shrink-0 rounded-full border border-gold-500 p-0.5 text-gold-600" />{highlight}</li>
                        ))}
                    </ul>
                </div>
            </div>
        </section>
    )
}
