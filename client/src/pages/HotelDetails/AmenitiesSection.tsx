import type { FacilityDto } from "@/api/hotels"
import { getFacilityIcon } from "./constants"

export function AmenitiesSection({ amenities }: { amenities: FacilityDto[] }) {
    return (
        <section id="amenities" className="scroll-mt-24 pt-9">
            <h2 className="font-heading text-xl font-semibold text-primary-900">Amenities</h2>
            <div className="mt-4 grid grid-cols-2 gap-2 sm:grid-cols-4">
                {amenities.map((facility) => {
                    const Icon = getFacilityIcon(facility.name)
                    return (
                        <div key={`${facility.category}-${facility.name}`} className="flex min-h-24 flex-col items-center justify-center rounded-lg border bg-bg-warm p-3 text-center">
                            <Icon size={24} strokeWidth={1.6} className="text-gold-600" />
                            <span className="mt-2 text-xs font-semibold text-primary-900">{facility.name}</span>
                        </div>
                    )
                })}
            </div>
        </section>
    )
}
