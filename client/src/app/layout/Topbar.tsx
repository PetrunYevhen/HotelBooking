import { useState } from "react"
import { Globe2, Menu, Search, UserRound, X } from "lucide-react"
import { Link, useLocation, useNavigate } from "react-router-dom"
import { BrandLogo } from "@/shared/components/BrandLogo"
import { getAuthSession } from "@/domains/auth/session"
import { clearAccessToken } from "@/shared/lib/api-client"

const navigation = [
    { label: "Destinations", href: "/#destinations" },
    { label: "Hotels", href: "/#hotels" },
    { label: "Deals", href: "/#deals" },
    { label: "Experiences", href: "/#experiences" },
    { label: "Membership", href: "/#membership" },
]

export function Topbar() {
    const [isMenuOpen, setIsMenuOpen] = useState(false)
    const location = useLocation()
    const authReturnTo = location.pathname === "/checkout" ? `${location.pathname}${location.search}${location.hash}` : ""
    const authQuery = authReturnTo ? `?returnTo=${encodeURIComponent(authReturnTo)}` : ""
    const navigate = useNavigate()
    const session = getAuthSession()
    const workspace = session?.role === "Admin" ? { label: "Admin", to: "/admin" } : session?.role === "Hotelier" ? { label: "Hotelier", to: "/hotelier" } : session ? { label: "Account", to: "/account" } : null
    const signOut = () => { clearAccessToken(); navigate("/") }

    return (
        <header className="sticky top-0 z-50 border-b bg-white/95 backdrop-blur">
            <div className="stayora-container flex h-[72px] items-center justify-between gap-6">
                <Link
                    to="/"
                    aria-label="Stayora home"
                    className="flex shrink-0 items-center"
                >
                    <BrandLogo />
                </Link>

                <nav aria-label="Primary navigation" className="hidden items-center gap-7 lg:flex">
                    {navigation.map((item) => (
                        <a
                            key={item.label}
                            href={item.href}
                            className="text-sm font-medium text-text-secondary transition-colors hover:text-primary-800"
                        >
                            {item.label}
                        </a>
                    ))}
                </nav>

                <div className="flex items-center gap-2 sm:gap-4">
                    <a
                        href="/#search"
                        aria-label="Search"
                        className="hidden items-center gap-2 text-sm font-medium text-text-secondary transition-colors hover:text-primary-800 sm:flex"
                    >
                        <Search size={18} strokeWidth={1.75} />
                        <span className="hidden xl:inline">Search</span>
                    </a>

                    <button
                        type="button"
                        aria-label="Change language"
                        className="hidden items-center gap-2 text-sm font-medium text-text-secondary transition-colors hover:text-primary-800 md:flex"
                    >
                        <Globe2 size={18} strokeWidth={1.75} />
                        EN
                    </button>

                    {session?.role === "Guest" && <Link
                        to="/my-booking"
                        className="hidden items-center gap-2 text-sm font-medium text-text-secondary transition-colors hover:text-primary-800 sm:flex"
                    >
                        <UserRound size={18} strokeWidth={1.75} />
                        My bookings
                    </Link>}

                    {workspace ? <><Link to={workspace.to}
                        className="hidden min-h-10 items-center rounded-md border border-primary-900 px-4 text-sm font-semibold text-primary-900 transition-colors hover:bg-primary-900 hover:text-white sm:inline-flex"
                    >{workspace.label}</Link><button onClick={signOut} className="hidden text-sm font-semibold text-text-secondary hover:text-primary-900 sm:inline">Sign out</button></> : <Link to={`/signin${authQuery}`}
                        className="hidden min-h-10 items-center rounded-md border border-primary-900 px-4 text-sm font-semibold text-primary-900 transition-colors hover:bg-primary-900 hover:text-white sm:inline-flex">Sign in</Link>}

                    <a
                        href="/#hotels"
                        className="hidden min-h-11 items-center rounded-md bg-primary px-5 text-sm font-semibold text-primary-foreground transition-colors hover:bg-[var(--color-primary-hover)] md:inline-flex"
                    >
                        Get Started
                    </a>

                    <button
                        type="button"
                        aria-label={isMenuOpen ? "Close menu" : "Open menu"}
                        aria-controls="mobile-navigation"
                        aria-expanded={isMenuOpen}
                        onClick={() => setIsMenuOpen((isOpen) => !isOpen)}
                        className="inline-flex size-11 items-center justify-center rounded-md border bg-white text-primary-800 lg:hidden"
                    >
                        {isMenuOpen ? <X size={22} strokeWidth={1.75} /> : <Menu size={22} strokeWidth={1.75} />}
                    </button>
                </div>
            </div>

            {isMenuOpen && (
                <div id="mobile-navigation" className="border-t bg-white px-4 py-4 shadow-lg lg:hidden">
                    <nav aria-label="Mobile navigation" className="stayora-container flex flex-col gap-1">
                        {navigation.map((item) => (
                            <a
                                key={item.label}
                                href={item.href}
                                onClick={() => setIsMenuOpen(false)}
                                className="rounded-md px-3 py-3 text-sm font-medium text-text-secondary hover:bg-muted hover:text-primary-800"
                            >
                                {item.label}
                            </a>
                        ))}
                        {session?.role === "Guest" && <Link
                            to="/my-booking"
                            onClick={() => setIsMenuOpen(false)}
                            className="mt-2 flex items-center gap-2 rounded-md bg-primary px-3 py-3 text-sm font-semibold text-white hover:bg-primary-800"
                        >
                            <UserRound size={18} strokeWidth={1.75} />
                            My bookings
                        </Link>}
                        {workspace ? <><Link to={workspace.to} onClick={() => setIsMenuOpen(false)} className="mt-1 flex items-center gap-2 rounded-md border border-primary-900 px-3 py-3 text-sm font-semibold text-primary-900 hover:bg-primary-900 hover:text-white"><UserRound size={18} />{workspace.label}</Link><button onClick={() => { signOut(); setIsMenuOpen(false) }} className="mt-1 rounded-md px-3 py-3 text-left text-sm font-semibold text-text-secondary">Sign out</button></> : <Link
                            to={`/signin${authQuery}`}
                            onClick={() => setIsMenuOpen(false)}
                            className="mt-1 flex items-center gap-2 rounded-md border border-primary-900 px-3 py-3 text-sm font-semibold text-primary-900 hover:bg-primary-900 hover:text-white"
                        >
                            <UserRound size={18} strokeWidth={1.75} />
                            Sign in
                        </Link>}
                    </nav>
                </div>
            )}
        </header>
    )
}
