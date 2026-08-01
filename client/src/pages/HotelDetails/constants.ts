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
import type { FacilityDto } from "@/api/hotels"

export const hotelImages = [
    "https://images.unsplash.com/photo-1566073771259-6a8506099945?auto=format&fit=crop&w=1600&q=85",
    "https://images.unsplash.com/photo-1582719508461-905c673771fd?auto=format&fit=crop&w=900&q=80",
    "https://images.unsplash.com/photo-1571896349842-33c89424de2d?auto=format&fit=crop&w=900&q=80",
    "https://images.unsplash.com/photo-1540555700478-4be289fbecef?auto=format&fit=crop&w=900&q=80",
    "https://images.unsplash.com/photo-1578683010236-d716f9a3f461?auto=format&fit=crop&w=900&q=80",
]

export const fallbackAmenities: FacilityDto[] = [
    { name: "Swimming pool", category: "Popular amenities" },
    { name: "Spa & wellness", category: "Popular amenities" },
    { name: "Free Wi-Fi", category: "Popular amenities" },
    { name: "Breakfast included", category: "Popular amenities" },
    { name: "Free parking", category: "Popular amenities" },
    { name: "Airport transfer", category: "Popular amenities" },
    { name: "Fitness center", category: "Popular amenities" },
    { name: "Pet friendly", category: "Popular amenities" },
]

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
