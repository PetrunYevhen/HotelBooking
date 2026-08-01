import logoImage from "@/assets/stayora.png"

interface BrandLogoProps {
    className?: string
}

export function BrandLogo({ className = "" }: BrandLogoProps) {
    return (
        <span
            aria-hidden="true"
            className={`relative block h-12 w-[180px] shrink-0 overflow-hidden ${className}`}
        >
            <img
                src={logoImage}
                alt=""
                className="pointer-events-none absolute -left-[67px] -top-[66px] h-[192px] w-72 max-w-none select-none"
            />
        </span>
    )
}
