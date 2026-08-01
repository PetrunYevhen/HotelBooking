import { Star } from "lucide-react"
import type { HotelDetailsDto } from "@/api/hotels"

export function ReviewsSection({ rating }: { rating: HotelDetailsDto["rating"] }) {
    return (
        <section id="reviews" className="mt-8 scroll-mt-24 border-t pt-8">
            <div className="flex flex-wrap items-end justify-between gap-4">
                <div><p className="section-eyebrow">Guest reviews</p><h2 className="section-title mt-1">What guests think</h2></div>
                <div className="flex items-center gap-3 rounded-lg bg-bg-warm px-5 py-3">
                    <span className="font-heading text-3xl font-semibold text-primary-900">{rating?.toFixed(1) ?? "New"}</span>
                    <div><div className="flex text-gold-500">{[0, 1, 2, 3, 4].map((star) => <Star key={star} size={14} className={rating && star < Math.round(rating) ? "fill-current" : ""} />)}</div><p className="mt-1 text-xs text-text-muted">Verified guest rating</p></div>
                </div>
            </div>
            <p className="mt-5 rounded-xl border bg-white p-6 text-sm text-text-secondary">Detailed guest reviews will appear here as soon as they are available.</p>
        </section>
    )
}

export function PoliciesSection({ checkIn, checkOut }: { checkIn: string; checkOut: string }) {
    return (
        <section id="policies" className="mt-8 scroll-mt-24 border-t pt-8">
            <p className="section-eyebrow">Good to know</p>
            <h2 className="section-title mt-1">Property policies</h2>
            <div className="mt-5 grid gap-3 sm:grid-cols-3">
                {[["Check-in / Check-out", `${checkIn} – ${checkOut}`], ["Cancellation policy", "Terms are shown before confirmation"], ["Secure booking", "Your booking details are protected"]].map(([title, copy]) => (
                    <div key={title} className="rounded-xl border bg-white p-5"><h3 className="text-sm font-semibold text-primary-900">{title}</h3><p className="mt-2 text-xs leading-5 text-text-secondary">{copy}</p></div>
                ))}
            </div>
        </section>
    )
}
