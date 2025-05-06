import { FC, useEffect, useState } from "react";
import { Button, Divider, Drawer, Image, Modal, Popconfirm, Space, Table, message } from 'antd';
import type { TableProps } from 'antd';
import { DeleteOutlined, EyeOutlined } from '@ant-design/icons';
import productService from "../../../../services/product-service";
import useModal from "../../../../hooks/useModal";
import CreateProductModal from "../../components/modals/CreateProductModal";
import { Link } from "react-router-dom";
import { ProductResource, PromotionResource } from "../../../../resources";
import { formatCurrencyVND } from "../../../../utils/format";
import TitleHeader from "../../components/TitleHeader";
import { ProductFilter } from "../../../customers/pages/ShopPage";
import AdvancedFilter from "../../../shared/AdvancedFilter";
import SortSelection from "../../../shared/SortSelection";
import useDebounce from "../../../../hooks/useDebounce";
import { QueryParams } from "./VariantManagement";
import { useSelector } from "react-redux";
import { selectAuth } from "../../../../feature/auth/authSlice";
import { TableRowSelection } from "antd/es/table/interface";
import AssignDiscountModal from "../../components/modals/AssignDiscountModal";
import promotionService from "../../../../services/promotion-service";

const intitialValues: ProductFilter = {
    pageIndex: 1,
    pageSize: 6,
}

const searchQueryInitialValue: QueryParams = {
    pageIndex: 1,
    pageSize: 6,
    searchString: ''
}

const ProductManagement: FC = () => {
    const [products, setProducts] = useState<ProductResource[]>([]);
    const { isModalOpen, handleCancel, handleOk, showModal } = useModal();
    const { isModalOpen: openPromotion, handleCancel: cancelPromotion, handleOk: okPromotion, showModal: showPromotion } = useModal();

    const [queryParams, setQueryParams] = useState<ProductFilter>(intitialValues)
    const [searchQuery, setSearchQuery] = useState<QueryParams>(searchQueryInitialValue)
    const [total, setTotal] = useState(6)
    const [open, setOpen] = useState(false);
    const debounceValue = useDebounce(searchQuery.searchString, 600)
    const [isSearch, setIsSearch] = useState(false);
    const { user } = useSelector(selectAuth);

    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);

    const showDrawer = () => {
        setOpen(true);
    };

    const onClose = () => {
        setOpen(false);
    };


    const fetchProducts = async (params: ProductFilter) => {
        const response = await productService.getAllProducts(params);
        setProducts(response.data);
        setTotal(response.pagination.totalItems)
        setIsSearch(false)
    }

    useEffect(() => {
        fetchProducts(queryParams);
    }, [queryParams])

    const searchProducts = async (params: QueryParams) => {
        const response = await productService.searchProducts(params);
        setProducts(response.data)
        setTotal(response.pagination.totalItems)
        setIsSearch(true)
    }

    useEffect(() => {
        searchProducts(searchQuery)
    }, [debounceValue])


    const handleSubmit = () => {
        fetchProducts(queryParams);
        handleOk();
    }

    const handleAssignPromotions = async (selectedPromotions: PromotionResource[]) => {
        const promotionIds = selectedPromotions.map(s => s.id);
        const productIds = selectedRowKeys.map(s => Number(s.toString()));
        console.log(productIds)
        const response = await promotionService.assignPromotionToProducts(promotionIds, productIds);
        if(response.success) {
            message.success(response.message);
            okPromotion()
        } else {
            message.error(response.message)
        }
    }

    const columns: TableProps<ProductResource>['columns'] = [
        {
            title: 'Ảnh',
            dataIndex: 'thumbnail',
            key: 'thumbnail',
            render(value) {
                return <Image src={value} preview={false} width='80px' height='80px' className="rounded-xl object-cover" />
            },
        },
        {
            title: 'Tên sản phẩm',
            dataIndex: 'name',
            key: 'name',
        },
        {
            title: 'Mô tả',
            dataIndex: 'description',
            key: 'description',
        },
        {
            title: 'Giá cũ',
            dataIndex: 'oldPrice',
            key: 'oldPrice',
            render(value) {
                return formatCurrencyVND(value)
            }
        },
        {
            title: 'Giá hiện tại',
            dataIndex: 'price',
            key: 'price',
            render(value) {
                return formatCurrencyVND(value)
            }
        },
        {
            title: 'Thao tác',
            key: 'action',
            render: (_, record) => user?.roles.includes('ADMIN') && (
                <Space size="middle">
                    <Popconfirm
                        title="Xóa sản phẩm"
                        description="Bạn có chắc là muốn xóa sản phẩm?"
                        onConfirm={() => confirmRemove(record.id)}
                        okText="Chắc chắn"
                        cancelText="Hủy bỏ"
                    >
                        <Button icon={<DeleteOutlined />} danger type="primary" size="small">Xóa</Button>
                    </Popconfirm>
                    <Link to={`/admin/product/${record.id}`}><Button icon={<EyeOutlined />} type="default" size="small">Chi tiết</Button></Link>
                </Space>
            ),
        },
    ];

    const confirmRemove = async (id: number) => {
        await productService.removeProduct(id)
        fetchProducts(queryParams)
        message.success('Xóa sản phẩm thành công');
    };


    const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
        console.log('selectedRowKeys changed: ', newSelectedRowKeys);
        setSelectedRowKeys(newSelectedRowKeys);
    };

    const rowSelection: TableRowSelection<ProductResource> = {
        selectedRowKeys,
        onChange: onSelectChange,
    };

    const hasSelected = selectedRowKeys.length > 0;

    return <div className="bg-white shadow-sm rounded-lg p-6">
        {hasSelected && <div className="p-2 flex items-center justify-between">
            <span className="font-semibold">Đã chọn {selectedRowKeys.length} sản phẩm</span>
            <Button onClick={showPromotion} type="primary">Áp dụng khuyến mãi</Button>
        </div>}

        {hasSelected && <Divider className="my-2" />}

        <Table
            columns={columns}
            dataSource={products}
            rowSelection={rowSelection}
            rowKey='id'
            title={() => <TitleHeader
                isShowFilterBtn
                title="Sản phẩm"
                callback={showModal}
                value={searchQuery.searchString}
                onChange={value => setSearchQuery({
                    ...searchQuery,
                    searchString: value
                })}
                isShowBtnTitle={user?.roles.includes('ADMIN')}
                onFilterClick={showDrawer}
                isShowSortSelection={true}
                sortSelection={<SortSelection
                    queryParams={queryParams}
                    onChange={params => setQueryParams({
                        ...queryParams,
                        ...params,
                        pageIndex: 1
                    })}
                />}
            />}
            pagination={{
                pageSize: isSearch ? searchQuery.pageSize : queryParams.pageSize,
                current: isSearch ? searchQuery.pageIndex : queryParams.pageIndex,
                onChange: (value) => {
                    if (isSearch) {
                        const updatedSearchQuery = {
                            ...searchQuery,
                            pageIndex: value
                        }
                        setSearchQuery(updatedSearchQuery)
                        searchProducts(updatedSearchQuery)

                    } else {
                        setQueryParams({
                            ...queryParams,
                            pageIndex: value
                        })
                    }
                },
                align: 'end',
                showLessItems: true,
                total: total
            }}
        />

        <Drawer title="LỌC NÂNG CAO" onClose={onClose} open={open}>
            <AdvancedFilter
                queryParams={queryParams}
                onChange={(params) => setQueryParams({
                    ...queryParams,
                    ...params,
                    pageIndex: 1
                })}
            />
        </Drawer>

        <Modal
            open={isModalOpen}
            onOk={handleOk}
            title={<p className="text-center font-semibold text-2xl">Thêm sản phẩm mới</p>}
            onCancel={handleCancel}
            width='800px'
            footer={[]}
        >
            <CreateProductModal handleOk={handleSubmit} />
        </Modal>

        <Modal
            open={openPromotion}
            onOk={okPromotion}
            title={<p className="text-center font-semibold text-2xl">Áp dụng khuyến mãi</p>}
            onCancel={cancelPromotion}
            width='800px'
            footer={[]}
        >
            <AssignDiscountModal onSubmit={handleAssignPromotions} />
        </Modal>
    </div>
};

export default ProductManagement;
