interface HotelGalleryProps {
    hotelName: string
    images: string[]
    onOpen: (image: string) => void
}

export function HotelGallery({ hotelName, images, onOpen }: HotelGalleryProps) {
    return (
        <section className="grid h-[360px] grid-cols-2 grid-rows-2 gap-2 overflow-hidden rounded-xl md:h-[480px] md:grid-cols-4">
            <GalleryImage src={images[0]} alt={`${hotelName} exterior`} className="col-span-2 row-span-2" onOpen={onOpen} />
            {images.slice(1).map((image, index) => (
                <GalleryImage key={image} src={image} alt={`${hotelName} gallery ${index + 2}`} className="hidden md:block" onOpen={onOpen} />
            ))}
        </section>
    )
}

function GalleryImage({ src, alt, className = "", onOpen }: { src: string; alt: string; className?: string; onOpen: (image: string) => void }) {
    return (
        <button type="button" onClick={() => onOpen(src)} className={`group relative overflow-hidden bg-muted ${className}`}>
            <img src={src} alt={alt} className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-[1.03]" />
        </button>
    )
}
