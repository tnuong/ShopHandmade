import { FC, useEffect, useState } from "react"
import { ProductResource } from "../../../resources"
import { Image, message, Tooltip } from "antd"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"
import { faHeart, faRemove } from "@fortawesome/free-solid-svg-icons"
import { Link } from "react-router-dom"
import { formatCurrencyVND } from "../../../utils/format"
import images from "../../../assets"
import productService from "../../../services/product-service"

type ProductWishlistProp = {
    product: ProductResource;
    onFetch: () => void
}

const ProductWishlist: FC<ProductWishlistProp> = ({
    product,
    onFetch
}) => {
    const [hover, setHover] = useState(false)
    const [hoverHeart, setHoverHeart] = useState(false)
    const [showImage, setShowImage] = useState(product.thumbnail);

    useEffect(() => {
        if (hover) setShowImage(product.zoomImage)
        else setShowImage(product.thumbnail)
    }, [hover])


    const handleRemoveWishlist = async () => {
        const response = await productService.removeWishlist(product.id);

        if (response.success) {
            message.success(response.message);
            onFetch()
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
            <Image preview={false} className={`md:w-[335px] md:h-[335px] w-[100px] h-[100px] object-cover transition-all duration-1000 ease-out ${hover && 'scale-110'}`} onError={() => setShowImage(images.demoMenth)} src={showImage} />
            <div className={`${hover ? 'top-2 opacity-100' : 'top-5 opacity-0'} transition-all duration-300 ease-in-out absolute right-2 flex flex-col gap-y-3`}>
                <Tooltip placement="left" title="Xóa khỏi wishlist">
                    <button
                        onClick={() => handleRemoveWishlist()}
                        onMouseLeave={() => setHoverHeart(false)}
                        onMouseOver={() => setHoverHeart(true)} className={`${hoverHeart ? 'bg-black text-white' : 'bg-white text-black'} transition-all duration-300 p-2 w-10 h-10 flex justify-center items-center rounded-md`}>
                        <FontAwesomeIcon icon={faRemove} />
                    </button>
                </Tooltip>
             
            </div>

        </div>

        <div className="flex flex-col gap-y-1 lg:gap-y-3 py-3">
            <Link to={`/product/${product.id}`} className="font-semibold">{product.name}</Link>
            <div className="flex flex-col lg:flex-row gap-x-4">
                <span className="line-through text-slate-600">{formatCurrencyVND(product.oldPrice)}</span>
                <span className="font-semibold">{formatCurrencyVND(product.price)}</span>
            </div>

        </div>
    </div >
}

export default ProductWishlist;