export function formatMoney(amount: number, currency: string) {
    try {
        return new Intl.NumberFormat(undefined, { style: "currency", currency, maximumFractionDigits: 2 }).format(amount)
    } catch {
        return `${amount.toFixed(2)} ${currency}`
    }
}

export function formatDate(value: string) {
    return new Date(`${value.slice(0, 10)}T00:00:00`).toLocaleDateString(undefined, { month: "short", day: "numeric", year: "numeric" })
}
