import { faSquare } from "@fortawesome/free-regular-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Button, Drawer, Image, Pagination, Popover, TreeDataNode } from "antd";
import { FilterOutlined } from '@ant-design/icons'
import React, { FC, useEffect, useState } from "react";
import images from "../../../assets";
import { CategoryLevelResource, ProductResource } from "../../../resources";
import productService from "../../../services/product-service";
import Product from "../components/Product";
import GridProductSkeleton from "../../shared/skeleton/GridProductSkeleton";
import AdvancedFilter from "../../shared/AdvancedFilter";
import SortSelection from "../../shared/SortSelection";


const bannerStyle: React.CSSProperties = {
    margin: 0,
    color: '#fff',
}



export type ProductFilter = {
    pageIndex: number;
    name?: string;
    pageSize: number;
    categoryIds?: number[];
    brandIds?: number[];
    colorIds?: number[];
    sizeIds?: number[];
    minPrice?: number;
    maxPrice?: number;
    sortBy?: "price" | "name"
    sortOrder?: "asc" | "desc"
}

const createCategoryTree = (categories: CategoryLevelResource[]): TreeDataNode[] => {
    return categories.map(category => ({
        key: category.id,
        title: category.name,
        children: category.categoryChildren ? createCategoryTree(category.categoryChildren) : null
    } as TreeDataNode))
}

const initialValues: ProductFilter = {
    pageIndex: 1,
    pageSize: 12,
    minPrice: 1,
    maxPrice: 10000000
}

const ShopPage: FC = () => {
    const [products, setProducts] = useState<ProductResource[]>([])
    const [params, setParams] = useState<ProductFilter>(initialValues);
    const [totalItems, setTotalItems] = useState(8)
    const [loading, setLoading] = useState(false)
    const [open, setOpen] = useState(false);

    const showDrawer = () => setOpen(true);
    const onClose = () => setOpen(false);

    const fetchProducts = async (queryParams: ProductFilter) => {
        const response = await productService.getAllProducts(queryParams);
        setProducts(response.data);
        setTotalItems(response.pagination.totalItems);
    }

    useEffect(() => {
        setLoading(true);
        fetchProducts(params);
        setTimeout(() => setLoading(false), 300);
    }, [params]);

    return (
        <div className="flex flex-col">
            {/* Banner */}
            

            {/* Filter + Sort */}
            <div className="px-6 mt-8">
                <div className="bg-white rounded-xl shadow-sm px-6 py-5 flex flex-col lg:flex-row justify-between items-start lg:items-center gap-4">
                    <div className="flex items-center gap-4">
                        <h2 className="text-xl font-semibold text-gray-800">Cửa hàng</h2>
                        <Button className="block lg:hidden" onClick={showDrawer} icon={<FilterOutlined />} type="default">Bộ lọc</Button>
                    </div>

                    <div className="flex items-center gap-4">
                        <Popover
                            arrow={false}
                            trigger="click"
                            title={<SortSelection
                                queryParams={params}
                                onChange={(queryParams: ProductFilter) => setParams({ ...params, ...queryParams })}
                            />}
                            placement="bottomRight"
                        >
                            <div className="flex items-center gap-2 cursor-pointer hover:text-blue-600 transition-colors">
                                <Image preview={false} src={images.sort} width="20px" />
                                <span className="text-base font-medium">Sắp xếp</span>
                            </div>
                        </Popover>
                        <span className="text-base text-gray-600">{products?.length} sản phẩm</span>
                    </div>
                </div>
            </div>

            {/* Main Content */}
            <div className="px-6 mt-6 grid grid-cols-12 gap-x-6">
                <div className="hidden lg:block lg:col-span-2">
                    <AdvancedFilter
                        queryParams={params}
                        onChange={(queryParams: ProductFilter) => setParams({ ...params, ...queryParams, pageIndex: 1 })}
                    />
                </div>

                <div className="col-span-12 lg:col-span-10">
                    {loading ? (
                        <GridProductSkeleton />
                    ) : (
                        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-x-4 gap-y-6">
                            {products.map(product => <Product key={product.id} product={product} />)}
                        </div>
                    )}

                    <div className="mt-4 flex justify-center">
                        {totalItems > params.pageSize && (
                            <Pagination
                                current={params.pageIndex}
                                onChange={(value) => setParams({ ...params, pageIndex: value })}
                                showLessItems
                                pageSize={params.pageSize}
                                total={totalItems}
                            />
                        )}
                    </div>
                </div>
            </div>

            {/* Drawer mobile filter */}
            <Drawer title="LỌC NÂNG CAO" onClose={onClose} open={open}>
                <AdvancedFilter
                    queryParams={params}
                    onChange={(params) => setParams({ ...params, pageIndex: 1 })}
                />
            </Drawer>
        </div>
    );
};

export default ShopPage;

