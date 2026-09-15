import { Routes, Route } from "react-router-dom"
import { Admin } from "@/workspaces/admin/AdminPanel"

export default function AdminRoutes() {
    return (
        <Routes>
            <Route index element={<Admin />} />
        </Routes>
    )
}
