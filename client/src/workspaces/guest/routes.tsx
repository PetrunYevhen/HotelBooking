import { Routes, Route } from "react-router-dom"
import { WorkspaceRoute } from "@/domains/auth/WorkspaceRoute"
import { Home } from "@/workspaces/guest/discovery/Home"
import { SearchResults } from "@/workspaces/guest/discovery/SearchResults"
import { HotelDetails } from "@/workspaces/guest/hotels/HotelDetails"
import { RoomDetails } from "@/workspaces/guest/hotels/RoomDetails"
import { Checkout } from "@/workspaces/guest/bookings/Checkout"
import { BookingConfirmation } from "@/workspaces/guest/bookings/BookingConfirmation"
import { MyBookings } from "@/workspaces/guest/bookings/MyBookings"
import { Account } from "@/workspaces/guest/account"

export default function GuestRoutes() {
    return (
        <Routes>
            <Route index element={<Home />} />
            <Route path="search" element={<SearchResults />} />
            <Route path="hotels/:id" element={<HotelDetails />} />
            <Route path="hotels/:hotelId/rooms/:roomId" element={<RoomDetails />} />
            <Route path="checkout" element={<Checkout />} />
            <Route path="booking-confirmation/:bookingId" element={<BookingConfirmation />} />
            <Route path="my-booking" element={<WorkspaceRoute roles={["Guest"]}><MyBookings /></WorkspaceRoute>} />
            <Route path="account" element={<WorkspaceRoute roles={["Guest"]}><Account /></WorkspaceRoute>} />
        </Routes>
    )
}
