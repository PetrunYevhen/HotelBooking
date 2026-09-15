interface RoomGalleryProps {
    roomName: string
    images: string[]
    onOpen: (image: string) => void
}

export function RoomGallery({ roomName, images, onOpen }: RoomGalleryProps) {
    return <section className="grid gap-2"><button type="button" onClick={() => onOpen(images[0])} className="group relative h-[330px] overflow-hidden rounded-xl bg-muted md:h-[460px]"><img src={images[0]} alt={roomName} className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-[1.03]" /><span className="absolute bottom-4 right-4 rounded-md bg-white px-3 py-2 text-xs font-semibold text-primary-900 shadow-sm">View all photos</span></button><div className="grid grid-cols-2 gap-2 sm:grid-cols-4">{images.slice(1).map((image, index) => <button key={image} type="button" onClick={() => onOpen(image)} className="group h-24 overflow-hidden rounded-lg bg-muted sm:h-28"><img src={image} alt={`${roomName} ${index + 2}`} className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-[1.03]" /></button>)}</div></section>
}
