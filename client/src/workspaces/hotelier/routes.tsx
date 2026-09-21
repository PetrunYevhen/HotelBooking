import { Routes, Route } from "react-router-dom"
import { HotelierHotelList } from "@/workspaces/hotelier/HotelList"
import { HotelierDashboard } from "@/workspaces/hotelier/HotelDashboard"
import { HotelPolicies } from "@/workspaces/hotelier/HotelPolicies"
import { CreateHotel } from "@/workspaces/hotelier/CreateHotel"

export default function HotelierRoutes() {
    return (
        <Routes>
            <Route index element={<HotelierHotelList />} />
            <Route path="hotels/new" element={<CreateHotel />} />
            <Route path="hotels/:hotelId/:section?" element={<HotelierDashboard />} />
            <Route path="hotels/:id/policies" element={<HotelPolicies />} />
        </Routes>
    )
}
