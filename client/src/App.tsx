import { Routes, Route } from "react-router-dom"
import { Layout } from "./components/layout/Layout"
import { Home } from "./pages/Home/index"
import { HotelDetails } from "./pages/HotelDetails/index"
import { HotelPolicies } from "./pages/HotelPolicies/index"
import { MyBookings } from "./pages/MyBookings/index"
import { RoomDetails } from "./pages/RoomDetails/index"
import { SearchResults } from "./pages/SearchResults/index"
import { Checkout } from "./pages/Checkout/index"
import { BookingConfirmation } from "./pages/BookingConfirmation/index"
import { HotelierDashboard, HotelierHotelList } from "./pages/Hotelier/index"
import { Login } from "./pages/Login/index"

function App() {
    return (
        <Routes>
            <Route element={<Layout />}>
                <Route path="/" element={<Home />} />
                <Route path="/search" element={<SearchResults />} />
                <Route path="/hotels/:id" element={<HotelDetails />} />
                <Route path="/hotels/:hotelId/rooms/:roomId" element={<RoomDetails />} />
                <Route path="/checkout" element={<Checkout />} />
                <Route path="/booking-confirmation/:bookingId" element={<BookingConfirmation />} />
                <Route path="/my-booking" element={<MyBookings />} />
                <Route path="/login" element={<Login />} />
                <Route path="/hotelier" element={<HotelierHotelList />} />
                <Route path="/hotelier/hotels/:hotelId/:section?" element={<HotelierDashboard />} />
                <Route path="/hotelier/hotels/:id/policies" element={<HotelPolicies />} />
            </Route>
        </Routes>
    )
}

export default App
