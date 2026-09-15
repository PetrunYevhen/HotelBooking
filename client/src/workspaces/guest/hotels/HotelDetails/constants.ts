import type { FacilityDto } from "@/domains/accommodations/api/hotels"

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
