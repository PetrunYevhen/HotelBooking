import type { ReactNode } from "react"

interface CardSectionProps {
    id?: string
    eyebrow?: string
    title: string
    actionLabel?: string
    actionHref?: string
    columns?: 4 | 5
    children: ReactNode
}

export function CardSection({
    id,
    eyebrow,
    title,
    actionLabel = "View all",
    actionHref = "#",
    columns = 4,
    children,
}: CardSectionProps) {
    const desktopColumns =
        columns === 5
            ? "lg:grid-cols-5"
            : "lg:grid-cols-4"

    return (
        <section id={id} className="py-10">
            <div className="mb-5 flex items-end justify-between gap-4">
                <div>
                    {eyebrow && (
                        <p className="section-eyebrow">
                            {eyebrow}
                        </p>
                    )}

                    <h2 className="section-title mt-1">
                        {title}
                    </h2>
                </div>

                <a
                    href={actionHref}
                    className="shrink-0 text-sm font-medium text-primary-700 transition-colors hover:text-primary-900"
                >
                    {actionLabel} →
                </a>
            </div>

            <div
                className={`
                      grid grid-flow-col
                      auto-cols-[minmax(260px,85%)]
                      gap-5 overflow-x-auto pb-4
                      sm:auto-cols-[minmax(270px,45%)]
                      lg:grid-flow-row lg:auto-cols-auto
                      lg:overflow-visible lg:pb-0
                      ${desktopColumns}
                  `}
            >
                {children}
            </div>
        </section>
    )
}
