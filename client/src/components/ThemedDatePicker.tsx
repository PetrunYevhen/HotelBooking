import { useEffect, useRef, useState } from "react"
import { ChevronDown, ChevronLeft, ChevronRight } from "lucide-react"

interface ThemedDatePickerProps {
    name: string
    value: string
    min: string
    placeholder: string
    onChange: (value: string) => void
}

const weekDays = ["Mo", "Tu", "We", "Th", "Fr", "Sa", "Su"]

function parseDate(value: string) {
    const [year, month, day] = value.split("-").map(Number)
    return new Date(year, month - 1, day)
}

function toInputValue(date: Date) {
    const year = date.getFullYear()
    const month = String(date.getMonth() + 1).padStart(2, "0")
    const day = String(date.getDate()).padStart(2, "0")
    return `${year}-${month}-${day}`
}

function sameDay(first: Date, second: Date) {
    return toInputValue(first) === toInputValue(second)
}

export function ThemedDatePicker({
    name,
    value,
    min,
    placeholder,
    onChange,
}: ThemedDatePickerProps) {
    const [isOpen, setIsOpen] = useState(false)
    const [visibleMonth, setVisibleMonth] = useState(() =>
        value ? parseDate(value) : parseDate(min),
    )
    const rootRef = useRef<HTMLDivElement>(null)
    const minimumDate = parseDate(min)
    const selectedDate = value ? parseDate(value) : null
    const today = new Date()

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

    function openCalendar() {
        setVisibleMonth(value ? parseDate(value) : parseDate(min))
        setIsOpen((current) => !current)
    }

    function changeMonth(offset: number) {
        setVisibleMonth(
            (current) =>
                new Date(current.getFullYear(), current.getMonth() + offset, 1),
        )
    }

    const year = visibleMonth.getFullYear()
    const month = visibleMonth.getMonth()
    const daysInMonth = new Date(year, month + 1, 0).getDate()
    const leadingEmptyDays = (new Date(year, month, 1).getDay() + 6) % 7
    const previousMonthLastDay = new Date(year, month, 0)
    const previousMonthDisabled = previousMonthLastDay < minimumDate
    const calendarDays = Array.from(
        { length: leadingEmptyDays + daysInMonth },
        (_, index) => {
            if (index < leadingEmptyDays) return null
            return new Date(year, month, index - leadingEmptyDays + 1)
        },
    )

    return (
        <div ref={rootRef} className="relative min-w-0 flex-1">
            <input type="hidden" name={name} value={value} />

            <button
                type="button"
                onClick={openCalendar}
                aria-haspopup="dialog"
                aria-expanded={isOpen}
                className="flex w-full items-center justify-between gap-2 text-left text-sm font-medium text-primary-900 outline-none"
            >
                <span className={value ? undefined : "text-text-muted"}>
                    {selectedDate
                        ? new Intl.DateTimeFormat("en-US", {
                              month: "short",
                              day: "numeric",
                              year: "numeric",
                          }).format(selectedDate)
                        : placeholder}
                </span>
                <ChevronDown
                    size={14}
                    className={`shrink-0 text-text-muted transition-transform ${isOpen ? "rotate-180" : ""}`}
                />
            </button>

            {isOpen && (
                <div
                    role="dialog"
                    aria-label="Choose a date"
                    className="absolute left-1/2 top-[calc(100%+1.25rem)] z-50 w-[min(320px,calc(100vw-3rem))] -translate-x-1/2 rounded-xl border border-gold-500/20 bg-white p-4 text-primary-900 shadow-lg sm:left-0 sm:translate-x-0"
                >
                    <div className="mb-4 flex items-center justify-between">
                        <button
                            type="button"
                            onClick={() => changeMonth(-1)}
                            disabled={previousMonthDisabled}
                            aria-label="Previous month"
                            className="inline-flex size-9 items-center justify-center rounded-full text-primary-800 transition-colors hover:bg-gold-100 disabled:cursor-not-allowed disabled:opacity-30"
                        >
                            <ChevronLeft size={18} />
                        </button>

                        <p className="font-heading text-base font-semibold text-primary-900">
                            {new Intl.DateTimeFormat("en-US", {
                                month: "long",
                                year: "numeric",
                            }).format(visibleMonth)}
                        </p>

                        <button
                            type="button"
                            onClick={() => changeMonth(1)}
                            aria-label="Next month"
                            className="inline-flex size-9 items-center justify-center rounded-full text-primary-800 transition-colors hover:bg-gold-100"
                        >
                            <ChevronRight size={18} />
                        </button>
                    </div>

                    <div className="grid grid-cols-7 gap-1">
                        {weekDays.map((day) => (
                            <span
                                key={day}
                                className="flex h-8 items-center justify-center text-[10px] font-semibold uppercase tracking-wide text-text-muted"
                            >
                                {day}
                            </span>
                        ))}

                        {calendarDays.map((date, index) => {
                            if (!date) {
                                return <span key={`empty-${index}`} />
                            }

                            const isDisabled = date < minimumDate
                            const isSelected = selectedDate
                                ? sameDay(date, selectedDate)
                                : false
                            const isToday = sameDay(date, today)

                            return (
                                <button
                                    key={toInputValue(date)}
                                    type="button"
                                    disabled={isDisabled}
                                    onClick={() => {
                                        onChange(toInputValue(date))
                                        setIsOpen(false)
                                    }}
                                    aria-label={new Intl.DateTimeFormat("en-US", {
                                        dateStyle: "long",
                                    }).format(date)}
                                    aria-pressed={isSelected}
                                    className={`flex size-9 items-center justify-center justify-self-center rounded-full text-sm transition-colors disabled:cursor-not-allowed disabled:text-text-disabled ${
                                        isSelected
                                            ? "bg-primary-900 font-semibold text-white shadow-sm"
                                            : "hover:bg-gold-100 hover:text-primary-900"
                                    } ${isToday && !isSelected ? "font-semibold text-gold-600 ring-1 ring-gold-500" : ""}`}
                                >
                                    {date.getDate()}
                                </button>
                            )
                        })}
                    </div>
                </div>
            )}
        </div>
    )
}
