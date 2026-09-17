import { NavLink, Outlet } from "react-router-dom"
import { cn } from "@/shared/lib/utils"

const tabs = [
  { to: "/admin/applications", label: "Hotelier applications" },
  { to: "/admin/properties", label: "Property management" },
]

export function AdminLayout() {
  return (
    <main className="stayora-container stayora-page">
      <p className="section-eyebrow">Admin workspace</p>
      <h1 className="section-title mt-1">Platform moderation</h1>
      <nav aria-label="Admin sections" className="mt-6 flex gap-1 overflow-x-auto rounded-xl border bg-white p-1">
        {tabs.map(tab => (
          <NavLink
            key={tab.to}
            to={tab.to}
            className={({ isActive }) =>
              cn(
                "inline-flex shrink-0 items-center gap-2 rounded-lg px-3 py-2 text-sm font-semibold",
                isActive ? "bg-primary-900 text-white" : "text-text-secondary hover:bg-bg-muted"
              )
            }
          >
            {tab.label}
          </NavLink>
        ))}
      </nav>
      <div className="mt-6">
        <Outlet />
      </div>
    </main>
  )
}
