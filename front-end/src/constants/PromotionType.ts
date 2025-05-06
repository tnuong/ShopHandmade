export const PromotionType = {
    FIXED_AMOUNT: "FIXED_AMOUNT",
    PERCENTAGE: "PERCENTAGE",
} as const;

export type PromotionTypeType = typeof PromotionType[keyof typeof PromotionType];