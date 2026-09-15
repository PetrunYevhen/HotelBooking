export function toLocalDateValue(date: Date) {
    const offset = date.getTimezoneOffset() * 60_000
    return new Date(date.getTime() - offset).toISOString().slice(0, 10)
}

export function addDays(value: string, days: number) {
    const date = new Date(`${value}T00:00:00`)
    date.setDate(date.getDate() + days)
    return toLocalDateValue(date)
}

export function calculateNights(checkIn: string, checkOut: string) {
    const start = new Date(`${checkIn}T00:00:00`).getTime()
    const end = new Date(`${checkOut}T00:00:00`).getTime()
    return Math.max(1, Math.round((end - start) / 86_400_000))
}
