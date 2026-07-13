import { useState } from "react"
import { Bed, Users } from "lucide-react"
import type { RoomDetailsDto } from "@/api/rooms"
import { createBooking } from "@/api/bookings"
import { GUEST_USER_ID } from "@/lib/constants"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"

interface RoomCardProps {
    room: RoomDetailsDto
    checkIn: string
    checkOut: string
}

export function RoomCard({ room, checkIn, checkOut }: RoomCardProps) {
    const [isFormOpen, setIsFormOpen] = useState(false)
    const [isSubmitting, setIsSubmitting] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [bookingId, setBookingId] = useState<string | null>(null)

    const [firstName, setFirstName] = useState("")
    const [lastName, setLastName] = useState("")
    const [email, setEmail] = useState("")
    const [phoneNumber, setPhoneNumber] = useState("")
    const [guestCount, setGuestCount] = useState(1)
    const [specialRequest, setSpecialRequest] = useState("")

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault()
        setIsSubmitting(true)
        setError(null)

        try {
            const id = await createBooking({
                hotelId: room.hotelId,
                roomId: room.roomId,
                userId: GUEST_USER_ID,
                checkIn,
                checkOut,
                guestCount,
                firstName,
                lastName,
                email,
                phoneNumber,
                specialRequest: specialRequest || undefined,
            })
            setBookingId(id)
            setIsFormOpen(false)
        } catch {
            setError("Could not create booking. Please check your details and try again.")
        } finally {
            setIsSubmitting(false)
        }
    }

    return (
        <div className="border rounded-xl overflow-hidden flex flex-col">
            <div className="flex">
                <img
                    src="https://placehold.co/200x160"
                    alt={room.type}
                    className="w-48 h-40 object-cover shrink-0"
                />

                <div className="p-4 flex flex-1 items-center justify-between">
                    <div>
                        <div className="flex items-center gap-2">
                            <span className="font-medium">{room.type}</span>
                            <span className="text-sm text-gray-500">№{room.roomNumber}</span>
                        </div>
                        <p className="text-sm text-gray-500 mt-1 flex items-center gap-3">
                            <span className="flex items-center gap-1">
                                <Bed size={14} /> {room.beds} beds
                            </span>
                            <span className="flex items-center gap-1">
                                <Users size={14} /> up to {room.capacity} guests
                            </span>
                        </p>
                        {room.description && <p className="text-sm text-gray-600 mt-2">{room.description}</p>}
                    </div>

                    <div className="text-right shrink-0">
                        <p className="text-sm">
                            <span className="font-semibold">
                                {room.effectivePriceAmount} {room.effectivePriceCurrency}
                            </span>
                            <span className="text-gray-500"> /night</span>
                        </p>
                        <button
                            disabled={!room.isActive}
                            onClick={() => setIsFormOpen((open) => !open)}
                            className="mt-2 px-4 py-1.5 rounded-lg bg-blue-600 text-white text-sm disabled:bg-gray-300"
                        >
                            Request to book
                        </button>
                    </div>
                </div>
            </div>

            {bookingId && (
                <p className="px-4 pb-4 text-sm text-green-600">
                    Booking created (id: {bookingId}). We'll email you a confirmation shortly.
                </p>
            )}

            {isFormOpen && (
                <form onSubmit={handleSubmit} className="border-t p-4 flex flex-col gap-3">
                    <div className="grid grid-cols-2 gap-3">
                        <div className="flex flex-col gap-1">
                            <Label htmlFor={`firstName-${room.roomId}`}>First name</Label>
                            <Input
                                id={`firstName-${room.roomId}`}
                                value={firstName}
                                onChange={(e) => setFirstName(e.target.value)}
                                required
                            />
                        </div>
                        <div className="flex flex-col gap-1">
                            <Label htmlFor={`lastName-${room.roomId}`}>Last name</Label>
                            <Input
                                id={`lastName-${room.roomId}`}
                                value={lastName}
                                onChange={(e) => setLastName(e.target.value)}
                                required
                            />
                        </div>
                        <div className="flex flex-col gap-1">
                            <Label htmlFor={`email-${room.roomId}`}>Email</Label>
                            <Input
                                id={`email-${room.roomId}`}
                                type="email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                required
                            />
                        </div>
                        <div className="flex flex-col gap-1">
                            <Label htmlFor={`phone-${room.roomId}`}>Phone number</Label>
                            <Input
                                id={`phone-${room.roomId}`}
                                value={phoneNumber}
                                onChange={(e) => setPhoneNumber(e.target.value)}
                                required
                            />
                        </div>
                        <div className="flex flex-col gap-1">
                            <Label htmlFor={`guests-${room.roomId}`}>Guests</Label>
                            <Input
                                id={`guests-${room.roomId}`}
                                type="number"
                                min={1}
                                max={room.capacity}
                                value={guestCount}
                                onChange={(e) => setGuestCount(Number(e.target.value))}
                                required
                            />
                        </div>
                    </div>
                    <div className="flex flex-col gap-1">
                        <Label htmlFor={`request-${room.roomId}`}>Special request (optional)</Label>
                        <Input
                            id={`request-${room.roomId}`}
                            value={specialRequest}
                            onChange={(e) => setSpecialRequest(e.target.value)}
                        />
                    </div>

                    {error && <p className="text-sm text-red-600">{error}</p>}

                    <Button type="submit" disabled={isSubmitting}>
                        {isSubmitting ? "Booking..." : "Confirm request"}
                    </Button>
                </form>
            )}
        </div>
    )
}
