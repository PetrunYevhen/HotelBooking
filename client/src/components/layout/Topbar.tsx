import { Search, Bell } from "lucide-react"

export function Topbar() {
    return (
        <header className="flex items-center justify-between px-6 py-4 border-b">
            <div className="flex items-center gap-2 border rounded-lg px-3 py-2 w-80">
                <Search size={18} className="text-gray-400"/>
                <input placeholder="Search..." className="outline-none w-full text-sm"/>
            </div>

            <div className="flex items-center gap-4">
                <Bell size={20} className="text-gray-500"/>
                <div className="flex items-center gap-2">
                    <div className="w-9 h-9 rounded-full bg-gray-200"/>
                    <span className="text-sm font-medium">user-name</span>
                </div>
            </div>
        </header>
    )
}
