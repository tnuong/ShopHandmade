import { FC, useEffect, useState } from "react";
import { Pagination as PaginationObject, ProductResource } from "../../../resources";
import productService from "../../../services/product-service";
import GridProductSkeleton from "../../shared/skeleton/GridProductSkeleton";
import ProductWishlist from "../components/ProductWishlist";
import { Pagination } from "antd";

const Wishlist: FC = () => {
    const [products, setProducts] = useState<ProductResource[]>([]);
    const [pagination, setPagination] = useState<PaginationObject>()

    const [loading, setLoading] = useState(false)

    const fetchProducts = async (pageIndex: number, pageSize: number) => {
        setLoading(true)
        const response = await productService.getWishlist(
            pageIndex,
            pageSize
        );


        console.log(response)
        setLoading(false)
        setProducts(response.data)
        setPagination(response.pagination);
    }

    useEffect(() => {
        fetchProducts(1, 8)
    }, [])

    return <div className="max-w-screen-xl w-full mx-auto flex flex-col items-start md:items-center gap-y-8 mt-4">
       
        {loading ? <GridProductSkeleton /> : <div className="grid grid-cols-4 gap-8">
            {products?.map(product => <ProductWishlist onFetch={() => fetchProducts(pagination?.pageIndex ?? 1, pagination?.pageSize ?? 8)} key={product.id} product={product} />)}
        </div>}

        <div className="mt-4 flex justify-center">
            <Pagination current={pagination?.pageIndex} onChange={(value) => {
                fetchProducts(value, pagination?.pageSize ?? 8);

            }} showLessItems pageSize={pagination?.pageSize} total={pagination?.totalItems} />
        </div>

    </div>
};

export default Wishlist;
