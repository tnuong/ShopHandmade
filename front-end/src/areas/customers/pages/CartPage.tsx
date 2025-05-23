import { FC, Fragment, useState } from "react";
import HeaderFadeIn from "../../../layouts/shared/HeaderFadeIn";
import Header from "../../../layouts/shared/Header";
import { Breadcrumb, Button, Divider, Empty } from "antd";
import CartItem from "../components/CartItem";
import Footer from "../../../layouts/shared/Footer";
import { useSelector } from "react-redux";
import { selectCart } from "../../../feature/cart/cartSlice";
import { Link, useNavigate } from "react-router-dom";
import { formatCurrencyVND } from "../../../utils/format";
import RequiredAuthentication from "../components/RequiredAuthentication";
import images from "../../../assets";
import { PromotionType } from "../../../constants/PromotionType";

const CartPage: FC = () => {
    const [fixed, setFixed] = useState(false)
    const { cartItems, total } = useSelector(selectCart)
    const navigate = useNavigate()

    const handleScroll = (event: React.UIEvent<HTMLDivElement>) => {
        const scrollTop = event.currentTarget.scrollTop;
        if (scrollTop >= 96) {
            setFixed(true)
        } else if (scrollTop === 0) {
            setFixed(false)
        }
    }

    const originalTotal = cartItems.reduce((sum, item) => {
        return sum + item.product.price * item.quantity;
    }, 0);

    const discountedTotal = cartItems.reduce((sum, item) => {
        let totalDiscount = 0;

        if (item.product.promotions?.length) {
            for (const promo of item.product.promotions) {
                if (promo.promotionType === PromotionType.FIXED_AMOUNT) {
                    totalDiscount += promo.discountValue;
                } else if (promo.promotionType === PromotionType.PERCENTAGE) {
                    totalDiscount += (item.product.price * promo.discountValue) / 100;
                }
            }
        }

        const finalPrice = Math.max(item.product.price - totalDiscount, 0);
        return sum + finalPrice * item.quantity;
    }, 0);

    const discountAmount = Math.max(originalTotal - discountedTotal, 0);
    
    return <div onScroll={handleScroll} className="h-screen overflow-y-auto bg-slate-50">
        {/* <HeaderFadeIn fixed={fixed} /> */}
        <Header />
        <div className="bg-slate-50 md:px-10 px-4">
            <Breadcrumb
                className="my-10 text-[17px]"
                separator=">"
                items={[
                    {
                        title: 'Trang chủ',
                        href: '/'
                    },
                    {
                        title: 'Giỏ hàng',
                        href: '',

                    },
                ]}
            />
            {
                cartItems.length > 0 ?
                    <div className="grid grid-cols-12 gap-y-6 md:gap-x-10">
                        <div className="col-span-12 md:col-span-8 flex flex-col">
                            <span className="mb-3 text-lg font-semibold">Giỏ hàng của bạn</span>
                            <div className="flex flex-col p-8 pb-0 rounded-2xl bg-white border-[1px] border-slate-100 shadow-sm">
                                {cartItems.map(cartItem => <Fragment key={cartItem.variant?.id}>
                                    <CartItem cartItem={cartItem} />
                                    <Divider />
                                </Fragment>)}
                            </div>
                        </div>
                        <div className="col-span-12 md:col-span-4 flex flex-col">
                            <span className="mb-3 text-lg font-semibold">Thông tin đơn giá</span>
                            <div className="flex flex-col gap-y-4 items-start p-8 rounded-2xl bg-white shadow-md">
                                <div className="w-full text-lg flex flex-col gap-y-3">
                                    <div className="flex justify-between">
                                        <span className="text-gray-500">Tạm tính</span>
                                        <span className="line-through text-gray-400">{formatCurrencyVND(originalTotal)}</span>
                                    </div>

                                    <div className="flex justify-between">
                                        <span className="text-green-600 font-medium">Tiết kiệm</span>
                                        <span className="text-green-600 font-semibold">- {formatCurrencyVND(discountAmount)}</span>
                                    </div>

                                    <Divider className="my-3" />

                                    <div className="flex justify-between items-center">
                                        <span className="text-gray-800 font-semibold text-[18px]">Tổng tiền</span>
                                        <span className="font-bold text-primary text-2xl">{formatCurrencyVND(discountedTotal)}</span>
                                    </div>
                                </div>

                                <RequiredAuthentication urlRedirectAfterAuthenticated='/checkout' handleIfAuthenticated={() => navigate('/checkout')}>
                                    <Button size="large" shape="round" className="mt-2 w-full text-[16px]" type="primary">Thanh toán</Button>
                                </RequiredAuthentication>
                            </div>

                        </div>
                    </div>
                    : <div className="flex flex-col items-center gap-y-8">
                        <Empty imageStyle={{ height: 200, marginBottom: 60 }} image={images.emptyCart} className="text-xl" description='Không có sản phẩm nào trong giỏ hàng' />
                        <Link to='/shop' className="w-full text-center">
                            <Button shape="default" size="large" type="primary">
                                MUA SẮM NGAY
                            </Button>
                        </Link>
                    </div>
            }

        </div>

        <Footer />
    </div>
};

export default CartPage;
