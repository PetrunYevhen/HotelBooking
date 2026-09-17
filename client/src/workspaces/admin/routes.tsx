import { Routes, Route, Navigate } from "react-router-dom"
import { AdminLayout } from "@/workspaces/admin/AdminLayout"
import { HotelierApplications } from "@/workspaces/admin/HotelierApplications"
import { PropertyManagement } from "@/workspaces/admin/PropertyManagement"

export default function AdminRoutes() {
  return (
    <Routes>
      <Route element={<AdminLayout />}>
        <Route index element={<Navigate to="applications" replace />} />
        <Route path="applications" element={<HotelierApplications />} />
        <Route path="properties" element={<PropertyManagement />} />
      </Route>
    </Routes>
  )
}
