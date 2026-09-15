import { Check } from "lucide-react"

const steps = ["Search", "Room", "Guests", "Payment", "Confirmation"]

export function BookingProgress({ current }: { current: "payment" | "confirmation" }) {
    const currentIndex = current === "payment" ? 3 : 4
    return <ol className="mx-auto flex max-w-3xl items-start justify-between gap-1" aria-label="Booking progress">
        {steps.map((step, index) => {
            const isComplete = index < currentIndex
            const isCurrent = index === currentIndex
            return <li key={step} className="flex min-w-0 flex-1 flex-col items-center text-center">
                <span className={`flex size-8 items-center justify-center rounded-full text-xs font-bold ${isComplete ? "bg-success-600 text-white" : isCurrent ? "bg-primary text-white ring-4 ring-primary/10" : "bg-muted text-text-muted"}`}>{isComplete ? <Check size={16} /> : index + 1}</span>
                <span className={`mt-2 truncate text-[11px] font-semibold sm:text-xs ${isCurrent ? "text-primary-900" : isComplete ? "text-success-600" : "text-text-muted"}`}>{step}</span>
            </li>
        })}
    </ol>
}
