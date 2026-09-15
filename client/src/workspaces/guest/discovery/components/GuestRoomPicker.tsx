import { useEffect, useRef, useState } from "react"
import { ChevronDown, Minus, Plus } from "lucide-react"

const MAX_ADULTS = 30
const MAX_CHILDREN = 10
const MAX_ROOMS = 10

interface CounterProps {
    label: string
    description: string
    value: number
    minimum: number
    maximum: number
    onChange: (value: number) => void
}

function Counter({
    label,
    description,
    value,
    minimum,
    maximum,
    onChange,
}: CounterProps) {
    return (
        <div className="flex items-center justify-between gap-5 py-3">
            <div>
                <p className="text-sm font-semibold text-primary-900">{label}</p>
                <p className="text-xs text-text-muted">{description}</p>
            </div>

            <div className="flex items-center gap-3">
                <button
                    type="button"
                    onClick={() => onChange(value - 1)}
                    disabled={value <= minimum}
                    aria-label={`Decrease ${label.toLowerCase()}`}
                    className="inline-flex size-8 items-center justify-center rounded-full border border-gold-500/40 text-primary-800 transition-colors hover:bg-gold-100 disabled:cursor-not-allowed disabled:border-border disabled:text-text-disabled"
                >
                    <Minus size={14} />
                </button>

                <span className="w-5 text-center text-sm font-semibold text-primary-900">
                    {value}
                </span>

                <button
                    type="button"
                    onClick={() => onChange(value + 1)}
                    disabled={value >= maximum}
                    aria-label={`Increase ${label.toLowerCase()}`}
                    className="inline-flex size-8 items-center justify-center rounded-full border border-gold-500/40 text-primary-800 transition-colors hover:bg-gold-100 disabled:cursor-not-allowed disabled:border-border disabled:text-text-disabled"
                >
                    <Plus size={14} />
                </button>
            </div>
        </div>
    )
}

export function GuestRoomPicker() {
    const [isOpen, setIsOpen] = useState(false)
    const [adults, setAdults] = useState(2)
    const [rooms, setRooms] = useState(1)
    const [childrenAges, setChildrenAges] = useState<number[]>([])
    const rootRef = useRef<HTMLDivElement>(null)

    useEffect(() => {
        function handlePointerDown(event: PointerEvent) {
            if (!rootRef.current?.contains(event.target as Node)) {
                setIsOpen(false)
            }
        }

        function handleKeyDown(event: KeyboardEvent) {
            if (event.key === "Escape") {
                setIsOpen(false)
            }
        }

        document.addEventListener("pointerdown", handlePointerDown)
        document.addEventListener("keydown", handleKeyDown)

        return () => {
            document.removeEventListener("pointerdown", handlePointerDown)
            document.removeEventListener("keydown", handleKeyDown)
        }
    }, [])

    function changeChildrenCount(count: number) {
        setChildrenAges((current) => {
            if (count > current.length) {
                return [...current, 5]
            }

            return current.slice(0, count)
        })
    }

    function changeChildAge(index: number, age: number) {
        setChildrenAges((current) =>
            current.map((currentAge, currentIndex) =>
                currentIndex === index ? age : currentAge,
            ),
        )
    }

    const guestCount = adults + childrenAges.length
    const summary = `${guestCount} ${guestCount === 1 ? "guest" : "guests"}, ${rooms} ${rooms === 1 ? "room" : "rooms"}`

    return (
        <div ref={rootRef} className="relative min-w-0 flex-1">
            <input type="hidden" name="adults" value={adults} />
            <input type="hidden" name="rooms" value={rooms} />
            {childrenAges.map((age, index) => (
                <input
                    key={index}
                    type="hidden"
                    name="childrenAges"
                    value={age}
                />
            ))}

            <button
                type="button"
                onClick={() => setIsOpen((current) => !current)}
                aria-haspopup="dialog"
                aria-expanded={isOpen}
                className="flex w-full items-center justify-between gap-2 text-left text-sm font-medium text-primary-900 outline-none"
            >
                <span className="truncate">{summary}</span>
                <ChevronDown
                    size={14}
                    className={`shrink-0 text-text-muted transition-transform ${isOpen ? "rotate-180" : ""}`}
                />
            </button>

            {isOpen && (
                <div
                    role="dialog"
                    aria-label="Choose guests and rooms"
                    className="absolute right-0 top-[calc(100%+1.25rem)] z-50 w-[min(360px,calc(100vw-3rem))] rounded-xl border border-gold-500/20 bg-white p-4 shadow-lg"
                >
                    <Counter
                        label="Adults"
                        description="Ages 18 or above"
                        value={adults}
                        minimum={1}
                        maximum={MAX_ADULTS}
                        onChange={setAdults}
                    />

                    <div className="border-t">
                        <Counter
                            label="Children"
                            description="Ages 0–17"
                            value={childrenAges.length}
                            minimum={0}
                            maximum={MAX_CHILDREN}
                            onChange={changeChildrenCount}
                        />
                    </div>

                    {childrenAges.length > 0 && (
                        <div className="grid grid-cols-2 gap-3 border-t py-4">
                            {childrenAges.map((age, index) => (
                                <label
                                    key={index}
                                    className="text-xs font-medium text-text-secondary"
                                >
                                    Child {index + 1} age
                                    <select
                                        value={age}
                                        onChange={(event) =>
                                            changeChildAge(
                                                index,
                                                Number(event.target.value),
                                            )
                                        }
                                        className="mt-1.5 w-full rounded-md border bg-white px-3 py-2 text-sm font-medium text-primary-900 outline-none transition-colors focus:border-gold-500 focus:ring-3 focus:ring-gold-500/15"
                                    >
                                        {Array.from({ length: 18 }, (_, childAge) => (
                                            <option key={childAge} value={childAge}>
                                                {childAge === 0
                                                    ? "Under 1 year"
                                                    : `${childAge} ${childAge === 1 ? "year" : "years"}`}
                                            </option>
                                        ))}
                                    </select>
                                </label>
                            ))}
                        </div>
                    )}

                    <div className="border-t">
                        <Counter
                            label="Rooms"
                            description="Number of rooms"
                            value={rooms}
                            minimum={1}
                            maximum={MAX_ROOMS}
                            onChange={setRooms}
                        />
                    </div>

                    <button
                        type="button"
                        onClick={() => setIsOpen(false)}
                        className="mt-2 w-full rounded-md bg-primary-900 px-4 py-2.5 text-sm font-semibold text-white transition-colors hover:bg-primary-800"
                    >
                        Done
                    </button>
                </div>
            )}
        </div>
    )
}
