import { lazy, Suspense } from "react"
import { Routes, Route } from "react-router-dom"
import { Layout } from "@/app/layout/Layout"
import { SignIn } from "@/domains/auth/SignIn"
import { WorkspaceRoute } from "@/domains/auth/WorkspaceRoute"
import GuestRoutes from "@/workspaces/guest/routes"

const HotelierRoutes = lazy(() => import("@/workspaces/hotelier/routes"))
const AdminRoutes = lazy(() => import("@/workspaces/admin/routes"))

function App() {
    return (
        <Routes>
            <Route element={<Layout />}>
                <Route path="/signin" element={<SignIn />} />
                <Route path="/hotelier/*" element={<WorkspaceRoute roles={["Hotelier"]}><Suspense fallback={null}><HotelierRoutes /></Suspense></WorkspaceRoute>} />
                <Route path="/admin/*" element={<WorkspaceRoute roles={["Admin"]}><Suspense fallback={null}><AdminRoutes /></Suspense></WorkspaceRoute>} />
                <Route path="/*" element={<GuestRoutes />} />
            </Route>
        </Routes>
    )
}

export default App
