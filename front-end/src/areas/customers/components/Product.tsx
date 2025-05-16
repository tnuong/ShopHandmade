import { Badge, Image, message, Modal, Tooltip } from "antd";
import { FC, useEffect, useState } from "react";
import {
    ShoppingCartOutlined
} from '@ant-design/icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEye, faHeart } from "@fortawesome/free-regular-svg-icons";
import { ProductResource, VariantResource } from "../../../resources";
import variantService from "../../../services/variant-service";
import { Link } from "react-router-dom";
import { formatCurrencyVND } from "../../../utils/format";
import useModal from "../../../hooks/useModal";
import QuickViewModal from "./modals/QuickViewModal";
import images from "../../../assets";
import { PromotionType } from "../../../constants/PromotionType";
import { faHeartBroken, faRemove } from "@fortawesome/free-solid-svg-icons";
import productService from "../../../services/product-service";
import { useSelector } from "react-redux";
import { selectAuth } from "../../../feature/auth/authSlice";

type ProductProps = {
    product: ProductResource
}

const Product: FC<ProductProps> = ({ product }) => {
    const hasPromotions = product.promotions.length > 0;

    return (
        <div className="relative">
            {hasPromotions && product.quantity > 0 && (
                <div className="absolute top-2 left-0 flex flex-col gap-1 z-10">
                    {product.promotions.map((promo) => (
                        <Tooltip
                            key={promo.id}
                            title={`${promo.name} | Giảm ${promo.promotionType === PromotionType.FIXED_AMOUNT
                                ? formatCurrencyVND(promo.discountValue)
                                : `${promo.discountValue}%`
                                }`}
                        >
                            <div className="relative -ml-1">
                                <div className="bg-red-500 text-white text-[13px] font-semibold py-1 px-2 pr-4 rounded-br-[4px] shadow-md skew-x-[-12deg] w-max">
                                    <span className="skew-x-[12deg] block">
                                        Giảm{" "}
                                        {promo.promotionType === PromotionType.FIXED_AMOUNT
                                            ? formatCurrencyVND(promo.discountValue)
                                            : `${promo.discountValue}%`}
                                    </span>
                                </div>
                                <div className="absolute top-0 left-0 w-0 h-0 border-t-[16px] border-t-red-500 border-l-[8px] border-l-transparent"></div>
                            </div>
                        </Tooltip>
                    ))}
                </div>
            )}

            {product.quantity > 0 ? (
                <ProductInner product={product} />
            ) : (
                <Badge.Ribbon placement="start" text="Hết hàng" color="volcano">
                    <ProductInner product={product} />
                </Badge.Ribbon>
            )}
        </div>
    );
};

export default Product;

const ProductInner: FC<ProductProps> = ({
    product
}) => {
    const [hover, setHover] = useState(false)
    const [hoverHeart, setHoverHeart] = useState(false)
    const [hoverQuickView, setHoverQuickView] = useState(false)
    const [variants, setVariants] = useState<VariantResource[]>([])
    const [selectVariant, setSelectVariant] = useState<VariantResource | null>(null);
    const [showImage, setShowImage] = useState(product.thumbnail);
    const { handleCancel, handleOk, isModalOpen, showModal } = useModal();
    const [hasWishlist, setHasWishlist] = useState(product.hasWishlist);

    const { isAuthenticated } = useSelector(selectAuth)



    useEffect(() => {
        const fetchVariants = async () => {
            const response = await variantService.getUniqueColorVariantsByProductId(product.id);
            const responseVariant = response.data;

            if (responseVariant.length > 0) {
                setSelectVariant(responseVariant[0])
            }

            setVariants(responseVariant)
        }

        fetchVariants();
    }, [])

    const handleSelectVariant = (variant: VariantResource) => {
        setSelectVariant(variant)
        setShowImage(variant.thumbnailUrl)
    }

    useEffect(() => {
        if (hover) setShowImage(product.zoomImage)
        else setShowImage(product.thumbnail)
    }, [hover])

    const handleAddWishlist = async () => {
        const response = await productService.addWishlist(product.id);

        if (response.success) {
            message.success(response.message);
            setHasWishlist(true)
        } else {
            message.error(response.message)
        }
    }

    const handleRemoveWishlist = async () => {
        const response = await productService.removeWishlist(product.id);

        if (response.success) {
            message.success(response.message);
            setHasWishlist(false)
        } else {
            message.error(response.message)
        }
    }


    return <div>
        <div
            onMouseLeave={() => setHover(false)}
            onMouseOver={() => setHover(true)}
            className="relative overflow-hidden"
        >
            <Image preview={false} width={'100%'} rootClassName='overflow-hidden' height={'100%'} className={`rounded-md aspect-square object-cover transition-all duration-1000 ease-out ${hover && 'scale-110'}`} onError={() => setShowImage(images.demoMenth)} src={showImage} />

            <div className={`${hover ? 'top-2 opacity-100' : 'top-5 opacity-0'} transition-all duration-300 ease-in-out absolute right-2 flex flex-col gap-y-3`}>
                {isAuthenticated && (hasWishlist ? <Tooltip placement="left" title="Xóa khỏi wishlist">
                    <button
                        onClick={() => handleRemoveWishlist()}
                        onMouseLeave={() => setHoverHeart(false)}
                        onMouseOver={() => setHoverHeart(true)} className={`${hoverHeart ? 'bg-black text-white' : 'bg-white text-black'} transition-all duration-300 p-2 w-10 h-10 flex justify-center items-center rounded-md`}>
                        <FontAwesomeIcon icon={faRemove} />
                    </button>
                </Tooltip> : <Tooltip placement="left" title="Thêm vào wishlist">
                    <button
                        onClick={() => handleAddWishlist()}
                        onMouseLeave={() => setHoverHeart(false)}
                        onMouseOver={() => setHoverHeart(true)} className={`${hoverHeart ? 'bg-black text-white' : 'bg-white text-black'} transition-all duration-300 p-2 w-10 h-10 flex justify-center items-center rounded-md`}>
                        <FontAwesomeIcon icon={faHeart} />
                    </button>
                </Tooltip>)}

                <Tooltip placement="left" title="Xem nhanh">
                    <button
                        onClick={showModal}
                        onMouseLeave={() => setHoverQuickView(false)}
                        onMouseOver={() => setHoverQuickView(true)} className={`${hoverQuickView ? 'bg-black text-white' : 'bg-white text-black'} transition-all duration-300 p-2 w-10 h-10 flex justify-center items-center rounded-md`}>
                        <FontAwesomeIcon icon={faEye} />
                    </button>
                </Tooltip>
            </div>



            {product.quantity > 0 && <button onClick={showModal} className={`${hover ? 'bottom-[8px] opacity-100' : '-bottom-10 opacity-0'} transition-all duration-500 ease-in-out flex items-center justify-center text-white text-[16px] py-2 gap-x-2 absolute left-0 right-0 bg-primary`}>
                <ShoppingCartOutlined />
                <span>+ Thêm vào giỏ hàng</span>
            </button>}
        </div>

        <div className="flex flex-col gap-y-1 lg:gap-y-3 py-3">
            <Link to={`/product/${product.id}`} className="font-semibold">{product.name}</Link>
            <div className="flex flex-col lg:flex-row gap-x-4">
                <span className="line-through text-slate-600">{formatCurrencyVND(product.oldPrice)}</span>
                <span className="font-semibold">{formatCurrencyVND(product.price)}</span>
            </div>
            <div className="flex gap-x-3">
                {variants.map(variant => <Tooltip key={variant.id} title={variant.color.name}>
                    <button
                        onClick={() => handleSelectVariant(variant)}
                        style={{
                            backgroundColor: variant.color.hexCode
                        }}
                        className="cursor-pointer w-5 h-5 border-[1px] border-gray-300 rounded-full"
                    >
                    </button>
                </Tooltip>)}

            </div>
        </div>

        <Modal
            open={isModalOpen}
            onOk={handleOk}
            onCancel={handleCancel}
            footer={[]}
            width='1000px'
        >
            <QuickViewModal productId={product.id} />
        </Modal>
    </div>
}