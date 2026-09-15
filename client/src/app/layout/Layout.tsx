import { Outlet } from "react-router-dom"
import { Topbar } from "./Topbar"
import { Footer } from "./Footer"

export function Layout() {
    return (
        <div className="flex min-h-screen flex-col bg-background text-foreground">
            <Topbar />

            <main className="flex-1">
                <Outlet />
            </main>

            <Footer />
        </div>
    )
}
