import { Routes, Route } from "react-router-dom"
import { Layout } from "./components/layout/Layout"
import { Home } from "./pages/Home"
import { HotelDetails } from "./pages/HotelDetails.tsx"
import { HotelPolicies } from "./pages/hotelier/HotelPolicies"
import { MyBookings } from "./pages/MyBookings"

function App() {
    return (
        <Routes>
            <Route element={<Layout />}>
                <Route path="/" element={<Home />} />
                <Route path="/hotels/:id" element={<HotelDetails />} />
                <Route path="/my-booking" element={<MyBookings />} />
                <Route path="/hotelier/hotels/:id/policies" element={<HotelPolicies />} />
            </Route>
        </Routes>
    )
}

export default App