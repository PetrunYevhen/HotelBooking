import { useState, type FormEvent, type ReactNode } from "react"
import { useNavigate } from "react-router-dom"
import { CalendarDays, MapPin, Search, Users } from "lucide-react"
import { GuestRoomPicker } from "@/components/GuestRoomPicker"
import { ThemedDatePicker } from "@/components/ThemedDatePicker"

const heroImage =
    "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?auto=format&fit=crop&w=2000&q=85"

function toLocalDateInputValue(date: Date) {
    const timezoneOffset = date.getTimezoneOffset() * 60_000
    return new Date(date.getTime() - timezoneOffset)
        .toISOString()
        .slice(0, 10)
}

function nextDate(dateValue: string) {
    const date = new Date(`${dateValue}T00:00:00`)
    date.setDate(date.getDate() + 1)
    return toLocalDateInputValue(date)
}

export function HomeHero() {
    const navigate = useNavigate()
    const [checkIn, setCheckIn] = useState("")
    const [checkOut, setCheckOut] = useState("")
    const today = toLocalDateInputValue(new Date())
    const minimumCheckOut = checkIn ? nextDate(checkIn) : today

    function handleCheckInChange(value: string) {
        setCheckIn(value)

        if (checkOut && checkOut <= value) {
            setCheckOut("")
        }
    }

    function handleSubmit(event: FormEvent<HTMLFormElement>) {
        event.preventDefault()

        const formData = new FormData(event.currentTarget)
        const destination = formData.get("destination")?.toString().trim()
        const adults = Number(formData.get("adults") ?? 1)
        const childrenAges = formData
            .getAll("childrenAges")
            .map((value) => value.toString())
        const childrenCount = childrenAges.length
        const rooms = Number(formData.get("rooms") ?? 1)

        const params = new URLSearchParams()
        if (destination) params.set("destination", destination)
        if (checkIn) params.set("checkIn", checkIn)
        if (checkOut) params.set("checkOut", checkOut)
        params.set("guests", String(adults + childrenCount))
        params.set("adults", String(adults))
        params.set("children", String(childrenCount))
        childrenAges.forEach((age) => params.append("childAge", age))
        params.set("rooms", String(rooms))

        navigate(`/search?${params.toString()}`)
    }

      return (
          <section className="relative z-10 isolate min-h-[620px] overflow-visible">
              <img
                  src={heroImage}
                  alt="Luxury coastal hotel"
                  className="absolute inset-0 -z-20 h-full w-full object-cover"
              />

              <div
                  aria-hidden="true"
                  className="absolute inset-0 -z-10 bg-gradient-to-r from-white/95 via-white/60 to-transparent"
              />

              <div className="stayora-container flex min-h-[620px] flex-col justify-center py-16">
                  <div className="max-w-[620px]">
                      <h1 className="font-heading text-4xl font-semibold leading-[1.08] text-primary-900 sm:text-5xl">
                          Find stays that
                          <br />
                          feel like{" "}
                          <span className="italic text-gold-500">
                              you
                          </span>
                      </h1>

                      <p className="mt-5 max-w-md text-base text-text-secondary sm:text-lg">
                          Handpicked hotels. Exclusive deals.
                          Memorable experiences.
                      </p>
                  </div>

                  <form
                      id="search"
                      onSubmit={handleSubmit}
                      className="mt-12 grid gap-3 rounded-xl bg-white p-4 shadow-lg lg:grid-cols-[1.3fr_1fr_1fr_1.1fr_auto]"
                  >
                      <SearchField
                          label="Destination"
                          icon={<MapPin size={18} />}
                      >
                          <input
                              type="text"
                              name="destination"
                              aria-label="Destination"
                              placeholder="Where to?"
                              className="w-full bg-transparent text-sm outline-none placeholder:text-text-muted"
                          />
                      </SearchField>

                      <SearchField
                          label="Check-in"
                          icon={<CalendarDays size={18} />}
                      >
                          <ThemedDatePicker
                              name="checkIn"
                              min={today}
                              value={checkIn}
                              placeholder="Add date"
                              onChange={handleCheckInChange}
                          />
                      </SearchField>

                      <SearchField
                          label="Check-out"
                          icon={<CalendarDays size={18} />}
                      >
                          <ThemedDatePicker
                              name="checkOut"
                              min={minimumCheckOut}
                              value={checkOut}
                              placeholder="Add date"
                              onChange={setCheckOut}
                          />
                      </SearchField>

                      <SearchField
                          label="Guests & rooms"
                          icon={<Users size={18} />}
                      >
                          <GuestRoomPicker />
                      </SearchField>

                      <button
                          type="submit"
                          className="flex min-h-12 items-center justify-center gap-2 rounded-md bg-primary px-6 text-sm font-semibold text-white transition-colors hover:bg-[var(--color-primary-hover)]"
                      >
                          <Search size={18} />
                          Search hotels
                      </button>
                  </form>
              </div>
          </section>
      )
}

interface SearchFieldProps {
    label: string
    icon: ReactNode
    children: ReactNode
}

function SearchField({ label, icon, children }: SearchFieldProps) {
    return (
        <div className="relative flex min-h-16 min-w-0 flex-col justify-center rounded-md border bg-white px-3.5 transition-colors focus-within:border-gold-500 focus-within:ring-3 focus-within:ring-gold-500/15">
            <span className="mb-1 text-[10px] font-semibold uppercase tracking-[0.06em] text-text-muted">
                {label}
            </span>

            <span className="flex min-w-0 items-center gap-2 text-primary-900 [&>svg]:shrink-0 [&>svg]:text-gold-600">
                {icon}
                {children}
            </span>
        </div>
    )
}
