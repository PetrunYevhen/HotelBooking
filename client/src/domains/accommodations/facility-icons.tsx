import {
    Car,
    Coffee,
    Dumbbell,
    PawPrint,
    Plane,
    Sparkles,
    UtensilsCrossed,
    Waves,
    Wifi,
} from "lucide-react"

export function getFacilityIcon(name: string) {
    const value = name.toLowerCase()
    if (value.includes("pool") || value.includes("swim")) return Waves
    if (value.includes("wi-fi") || value.includes("wifi") || value.includes("internet")) return Wifi
    if (value.includes("breakfast") || value.includes("coffee")) return Coffee
    if (value.includes("restaurant") || value.includes("dining")) return UtensilsCrossed
    if (value.includes("parking") || value.includes("car")) return Car
    if (value.includes("airport") || value.includes("transfer")) return Plane
    if (value.includes("fitness") || value.includes("gym")) return Dumbbell
    if (value.includes("pet")) return PawPrint
    return Sparkles
}
