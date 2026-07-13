import { Heart, Star} from "lucide-react";
import { Link } from "react-router-dom";

interface HotelCardProps {
    hotelId: string;
    name: string;
    location: string;
    pricePerNight: number;
    rating: number;
    imageUrl: string;
}

export function HotelCard({ hotelId, name, location, pricePerNight, imageUrl, rating}: HotelCardProps) {
    return (
        <div className="relative rounded-xl overflow-hidden border w-64 shrink-0">
            <Link to={`/hotels/${hotelId}`} className="block">
                <div className="h-36">
                    <img src={imageUrl} alt={name} className="w-full h-full object-cover" />
                </div>
                <div className="p-3">
                    <div className="flex items-center justify-between">
                        <span className="font-medium text-sm">{name}</span>
                        <span className="flex items-center gap-1 text-sm">
            <Star size={14} className="fill-yellow-400 text-yellow-400" />
                            {rating}
          </span>
                    </div>
                    <p className="text-gray-500 text-xs mt-1">{location}</p>
                    <p className="mt-2 text-sm">
                        <span className="font-semibold">${pricePerNight}</span>
                        <span className="text-gray-500"> /night</span>
                    </p>
                </div>
            </Link>
            <button className="absolute top-2 right-2 bg-white/80 rounded-full p-1.5">
                <Heart size={16} />
            </button>
        </div>
    )
}