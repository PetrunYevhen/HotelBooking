import type { FormEvent, ReactNode } from "react"
import { Link } from "react-router-dom"
import { BrandLogo } from "@/components/BrandLogo"
import {
    AtSign,
    Camera,
    Crown,
    Play,
    Send,
    Users,
} from "lucide-react"

const companyLinks = [
    "About us",
    "Careers",
    "Press",
    "Sustainability",
    "Partner with us",
]

const supportLinks = [
    "Help center",
    "FAQs",
    "Booking guide",
    "Contact us",
    "Travel requirements",
]

const policyLinks = [
    "Terms & conditions",
    "Privacy policy",
    "Cancellation policy",
    "Cookie policy",
    "Security",
]

export function Footer() {
    function handleNewsletterSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        const formData = new FormData(event.currentTarget)
        const email = formData.get("email")

        console.log("Newsletter email:", email)
    }

    return (
        <footer
            id="membership"
            className="relative mt-24 bg-primary-900 text-white"
        >
            <div className="stayora-container">
                <MembershipBanner />

                <div className="grid gap-10 pb-10 pt-2 md:grid-cols-2 lg:grid-cols-[1.3fr_1fr_1fr_1fr_1.4fr]">
                    <FooterBrand />

                    <FooterColumn
                        title="Company"
                        links={companyLinks}
                    />

                    <FooterColumn
                        title="Support"
                        links={supportLinks}
                    />

                    <FooterColumn
                        title="Policies"
                        links={policyLinks}
                    />

                    <section>
                        <h2 className="text-sm font-semibold uppercase tracking-[0.06em] text-gold-400">
                            Newsletter
                        </h2>

                        <p className="mt-4 text-sm leading-6 text-white/70">
                            Get the best travel deals and inspiration.
                        </p>

                        <form
                            onSubmit={handleNewsletterSubmit}
                            className="mt-4 flex overflow-hidden rounded-md border border-white/20 bg-white/5 focus-within:border-gold-400"
                        >
                            <label
                                htmlFor="newsletter-email"
                                className="sr-only"
                            >
                                Email address
                            </label>

                            <input
                                id="newsletter-email"
                                name="email"
                                type="email"
                                required
                                placeholder="Enter your email"
                                className="min-w-0 flex-1 bg-transparent px-4 py-3 text-sm text-white outline-none placeholder:text-white/45"
                            />

                            <button
                                type="submit"
                                aria-label="Subscribe to newsletter"
                                className="m-1 inline-flex size-10 shrink-0 items-center justify-center rounded-sm bg-white text-primary-900 transition-colors hover:bg-gold-100"
                            >
                                <Send size={17} strokeWidth={1.75} />
                            </button>
                        </form>
                    </section>
                </div>

                <FooterBottom />
            </div>
        </footer>
    )
}

function MembershipBanner() {
    return (
        <section className="relative -top-12 grid items-center gap-6 rounded-xl border border-gold-500/40 bg-primary-800 px-6 py-7 shadow-lg md:grid-cols-[auto_1fr_auto] md:px-10">
            <div className="flex size-16 items-center justify-center rounded-full border border-gold-500/40 bg-gold-500/10 text-gold-400">
                <Crown size={30} strokeWidth={1.5} />
            </div>

            <div>
                <h2 className="font-heading text-2xl font-semibold text-white">
                    Unlock exclusive benefits with Stayora Membership
                </h2>

                <p className="mt-2 text-sm text-white/70">
                    Member-only prices, free upgrades, early access
                    to deals, and more.
                </p>
            </div>

            <div className="flex flex-col items-start gap-2 md:items-center">
                <a
                    href="#join"
                    className="inline-flex min-h-11 min-w-48 items-center justify-center rounded-md bg-gold-400 px-6 text-sm font-semibold text-primary-900 transition-colors hover:bg-gold-500"
                >
                    Join now
                </a>

                <p className="text-xs text-white/65">
                    Already a member?{" "}
                    <Link
                        to="/my-booking"
                        className="font-medium text-white underline underline-offset-4 hover:text-gold-400"
                    >
                        My bookings
                    </Link>
                </p>
            </div>
        </section>
    )
}

function FooterBrand() {
    return (
        <section>
            <a
                href="/"
                aria-label="Stayora home"
                className="inline-flex items-center"
            >
                <BrandLogo />
            </a>

            <p className="mt-4 max-w-56 text-sm leading-6 text-white/70">
                Your journey, elevated.
                <br />
                Handpicked stays. Unmatched experiences.
            </p>

            <div className="mt-5 flex items-center gap-3">
                <SocialLink
                    href="#instagram"
                    label="Instagram"
                    icon={<Camera size={17} />}
                />

                <SocialLink
                    href="#facebook"
                    label="Facebook"
                    icon={<Users size={16} />}
                />

                <SocialLink
                    href="#twitter"
                    label="X"
                    icon={<AtSign size={16} />}
                />

                <SocialLink
                    href="#youtube"
                    label="YouTube"
                    icon={<Play size={18} />}
                />
            </div>
        </section>
    )
}

interface FooterColumnProps {
    title: string
    links: string[]
}

function FooterColumn({
                          title,
                          links,
                      }: FooterColumnProps) {
    return (
        <section>
            <h2 className="text-sm font-semibold uppercase tracking-[0.06em] text-gold-400">
                {title}
            </h2>

            <ul className="mt-4 space-y-2.5">
                {links.map((link) => (
                    <li key={link}>
                        <a
                            href="#"
                            className="text-sm text-white/70 transition-colors hover:text-white"
                        >
                            {link}
                        </a>
                    </li>
                ))}
            </ul>
        </section>
    )
}

interface SocialLinkProps {
    href: string
    label: string
    icon: ReactNode
}

function SocialLink({
                        href,
                        label,
                        icon,
                    }: SocialLinkProps) {
    return (
        <a
            href={href}
            aria-label={label}
            className="inline-flex size-9 items-center justify-center rounded-full border border-white/15 text-white/75 transition-colors hover:border-gold-400 hover:text-gold-400"
        >
            {icon}
        </a>
    )
}

function FooterBottom() {
    return (
        <div className="flex flex-col gap-5 border-t border-white/10 py-6 text-xs text-white/50 md:flex-row md:items-center md:justify-between">
            <p>© 2026 Stayora. All rights reserved.</p>

            <div
                aria-label="Supported payment methods"
                className="flex flex-wrap items-center gap-2"
            >
                <PaymentBadge label="VISA" />
                <PaymentBadge label="Mastercard" />
                <PaymentBadge label="AMEX" />
                <PaymentBadge label="PayPal" />
                <PaymentBadge label="Apple Pay" />
                <PaymentBadge label="Google Pay" />
            </div>
        </div>
    )
}

function PaymentBadge({
                          label,
                      }: {
    label: string
}) {
    return (
        <span className="rounded-sm border border-white/15 bg-white/5 px-2 py-1 font-medium text-white/70">
              {label}
          </span>
    )
}
