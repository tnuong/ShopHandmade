import { FC, useEffect, useState } from "react";
import QuantityButton from "./QuantityButton";
import { Button, Image, Popconfirm } from "antd";
import { CartItemResource } from "../../../resources";
import { useDispatch } from "react-redux";
import { DeleteOutlined } from '@ant-design/icons';
import { AppDispatch } from "../../../app/store";
import { descreaseQuantity, increaseQuantity, removeItem } from "../../../feature/cart/cartSlice";
import { formatCurrencyVND } from "../../../utils/format";
import { PromotionType } from "../../../constants/PromotionType";

type CartItemProps = {
    cartItem: CartItemResource;
};

const CartItem: FC<CartItemProps> = ({ cartItem }) => {
    const dispatch = useDispatch<AppDispatch>();

    const [discountValue, setDiscountValue] = useState<number>(0);
    const [priceAfterDiscount, setPriceAfterDiscount] = useState<number>(0);

    useEffect(() => {
        let totalDiscount = 0;

        // Tính tổng số tiền giảm theo khuyến mãi
        if (cartItem.product.promotions && cartItem.product.promotions.length > 0) {
            for (const promo of cartItem.product.promotions) {
                if (promo.promotionType === PromotionType.FIXED_AMOUNT) {
                    totalDiscount += promo.discountValue;
                } else if (promo.promotionType === PromotionType.PERCENTAGE) {
                    totalDiscount += (cartItem.product.price * promo.discountValue) / 100;
                }
            }
        }

        // Tính tổng giá sản phẩm * số lượng
        const totalPrice = cartItem.product.price * cartItem.quantity;

        // Nếu tổng số tiền giảm * số lượng lớn hơn giá sản phẩm * số lượng, điều chỉnh số tiền giảm
        const totalDiscountWithQuantity = totalDiscount * cartItem.quantity;
        const adjustedDiscount = totalDiscountWithQuantity > totalPrice ? totalPrice : totalDiscountWithQuantity;

        // Tính giá sau khi giảm
        const finalPrice = Math.max(totalPrice - adjustedDiscount, 0);

        setDiscountValue(adjustedDiscount);
        setPriceAfterDiscount(finalPrice);
    }, [cartItem]);

    return (
        <div className="flex justify-between items-center gap-x-2">
            <div className="flex gap-x-2 items-center">
                <Image
                    preview={false}
                    className="rounded-md object-cover"
                    width="80px"
                    height="80px"
                    src={cartItem.variant?.thumbnailUrl}
                />
                <div className="flex text-sm flex-col lg:gap-y-2 gap-y-1">
                    <span className="font-semibold text-[16px]">{cartItem.product?.name}</span>
                    <span>Màu sắc: <b>{cartItem.variant?.color?.name}</b></span>
                    <span>Kích cỡ: <b>{cartItem.variant?.size?.eSize}</b></span>
                </div>
            </div>

            <div className="flex flex-col gap-y-2 items-center">
                <div className="flex gap-x-3 items-center">
                    <QuantityButton
                        size="small"
                        defaultValue={cartItem.quantity}
                        onIncrease={() => dispatch(increaseQuantity(cartItem.variant.id))}
                        onDescrease={() => dispatch(descreaseQuantity(cartItem.variant.id))}
                    />
                </div>
                <div className="hidden lg:flex flex-col items-center text-right gap-y-1">
                    {discountValue > 0 ? (
                        <>
                            <span className="text-primary text-[15px] font-semibold">
                                {formatCurrencyVND(priceAfterDiscount)}
                            </span>
                            <span className="text-red-500 text-xs italic">
                            Đã giảm:  -{formatCurrencyVND(discountValue)}
                            </span>
                        </>
                    ) : (
                        <span className="text-primary font-semibold text-[15px]">
                            {formatCurrencyVND(cartItem.product?.price * cartItem.quantity)}
                        </span>
                    )}
                </div>
            </div>

            <Popconfirm
                title="Xóa sản phẩm"
                description="Bạn muốn xóa sản phẩm khỏi giỏ hàng ư?"
                onConfirm={() => dispatch(removeItem(cartItem.variant.id))}
                okText="Có"
                cancelText="Không"
            >
                <Button size="small" icon={<DeleteOutlined />} type="primary" danger />
            </Popconfirm>
        </div>
    );
};

export default CartItem;
