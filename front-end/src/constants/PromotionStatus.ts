export const PromotionStatus = {
    INACITVE: "INACITVE",
    ACTIVE: "ACTIVE",
    EXPIRED: "EXPIRED",
} as const;

export type PromotionStatusType = typeof PromotionStatus[keyof typeof PromotionStatus];