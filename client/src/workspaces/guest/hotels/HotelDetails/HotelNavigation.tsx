const tabs = [
    ["Overview", "overview"],
    ["Rooms", "rooms"],
    ["Amenities", "amenities"],
    ["Reviews", "reviews"],
    ["Location", "location"],
    ["Policies", "policies"],
] as const

export function HotelNavigation({ activeTab, onNavigate }: { activeTab: string; onNavigate: (target: string) => void }) {
    return (
        <nav aria-label="Hotel sections" className="flex gap-7 overflow-x-auto border-b text-sm font-medium text-text-secondary">
            {tabs.map(([label, target]) => (
                <a
                    key={target}
                    href={`#${target}`}
                    onClick={() => onNavigate(target)}
                    className={`shrink-0 border-b-2 py-3 transition-colors hover:text-primary-900 ${activeTab === target ? "border-primary-900 text-primary-900" : "border-transparent"}`}
                >
                    {label}
                </a>
            ))}
        </nav>
    )
}
