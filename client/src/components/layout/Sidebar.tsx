import { Link, useLocation } from "react-router-dom";
import { Home, Calendar, MessageSquare, User, Settings, HelpCircle, LogOut, Heart, LayoutDashboard } from "lucide-react";

const navItems = [
    {to: "/", label: "Home", icon: Home},
    { to: "/my-booking", label: "My Booking", icon: Calendar },
    { to: "/my-favorites", label: "My Favorites", icon: Heart },
    { to: "/messages", label: "Messages", icon: MessageSquare },
    { to: "/admin", label: "Admin Panel", icon: LayoutDashboard },
]

const bottomItems = [
    { to: "/profile", label: "Profile", icon: User },
    { to: "/settings", label: "Settings", icon: Settings },
    { to: "/help", label: "Help and Support", icon: HelpCircle },
]

export function Sidebar() {
    const location = useLocation();

    return (
        <aside className="w-64 border-r h-screen flex flex-col justify-between p-4">
            <nav className="flex flex-col gap-1">
                {navItems.map((item) => (
                    <SidebarLink key={item.to} item={item} active={location.pathname === item.to} />
                ))}
            </nav>

            <nav className="flex flex-col gap-1">
                {bottomItems.map((item) => (
                    <SidebarLink key={item.to} item={item} active={location.pathname === item.to} />
                ))}
                <button className="flex items-center gap-3 px-3 py-2 rounded-lg text-red-500 hover:bg-red-50">
                    <LogOut size={20} />
                    Logout
                </button>
            </nav>
        </aside>
    )
}


function SidebarLink({ item, active}: { item: typeof navItems[number], active: boolean }) {
    const Icon = item.icon
    return (
        <Link
            to={item.to}
            className={`flex items-center gap-3 px-3 py-2 rounded-lg ${
                active ? "bg-blue-50 text-blue-600" : "text-gray-600 hover:bg-gray-100"
            }`}
        >
            <Icon size={20} />
            {item.label}
        </Link>
    )
}
