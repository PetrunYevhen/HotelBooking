export const statusStyles: Record<string, string> = {
    Pending: "bg-warning-100 text-warning-600",
    Confirmed: "bg-info-100 text-info-600",
    CheckedIn: "bg-gold-100 text-gold-600",
    Completed: "bg-success-100 text-success-600",
    Cancelled: "bg-error-100 text-error-600",
    NoShow: "bg-error-100 text-error-600",
}

export const cancellableStatuses = new Set(["Pending", "Confirmed"])
