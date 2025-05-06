import { FC, useEffect, useState } from "react";
import { Button, Modal, Popconfirm, Space, Table, Tag, message } from 'antd';
import type { TableProps } from 'antd';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons';
import useModal from "../../../../hooks/useModal";
import { PromotionResource } from "../../../../resources";
import CreateBrandModal from "../../components/modals/CreateBrandModal";
import brandService from "../../../../services/brand-service";
import EditBrandModal from "../../components/modals/EditBrandModal";
import TitleHeader from "../../components/TitleHeader";
import useDebounce from "../../../../hooks/useDebounce";
import { QueryParams } from "../products/VariantManagement";
import promotionService from "../../../../services/promotion-service";
import CreatePromotionModal from "../../components/modals/CreatePromotionModal";
import { formatDateTime } from "../../../../utils/format";
import { PromotionType } from "../../../../constants/PromotionType";

const intitialValues: QueryParams = {
    pageIndex: 1,
    pageSize: 8,
    searchString: ''
}

const PromotionManagement: FC = () => {
    const { isModalOpen, handleCancel, handleOk, showModal } = useModal()
    const { isModalOpen: isEditModalOpen, handleCancel: edithandleCancel, handleOk: edithandleOk, showModal: editshowModal } = useModal()
    const [promotion, setPromotion] = useState<PromotionResource | null>(null)
    const [promotions, setPromotions] = useState<PromotionResource[]>([]);
    const [queryParams, setQueryParams] = useState<QueryParams>(intitialValues)
    const [total, setTotal] = useState(8)
    const debounceValue = useDebounce(queryParams.searchString, 600)

    const fetchPromotions = async (params: QueryParams) => {
        const response = await promotionService.getAllPromotions(params);
        setPromotions(response.data)
        setTotal(response.pagination.totalItems)
    }

    const confirmRemove = async (id: number) => {
        const response = await promotionService.removePromotion(id);

        if(response.success) {
            fetchPromotions(queryParams)
            message.success('Xóa khuyến mại thành công');
        } else {
            message.error(response.message)
        }
       
    };

    const handleSubmit = () => {
        edithandleOk();
        handleOk();
        fetchPromotions(queryParams)
    }


    useEffect(() => {
        fetchPromotions(queryParams)
    }, [debounceValue]);


    const columns: TableProps<PromotionResource>['columns'] = [
        {
            title: 'Tên khuyến mại',
            dataIndex: 'name',
            key: 'name',
        },
        {
            title: 'Nội dung',
            dataIndex: 'description',
            key: 'description',
        },
        {
            title: 'Loại khuyến mại',
            dataIndex: 'promotionType',
            key: 'promotionType',
            render: (value) => {
                if(value === PromotionType.FIXED_AMOUNT) {
                    return <Tag color="yellow">Số tiền cố định</Tag>
                }

                return <Tag color="green">Phần trăm</Tag>
            }
        },
        {
            title: 'Giá trị giảm',
            dataIndex: 'discountValue',
            key: 'discountValue',
        },
        {
            title: 'Bắt đầu',
            dataIndex: 'fromDate',
            key: 'fromDate',
            render: (value) => formatDateTime(new Date(value))
        },
        {
            title: 'Kết thúc',
            dataIndex: 'toDate',
            key: 'toDate',
            render: (value) => formatDateTime(new Date(value))
        },
        {
            title: 'Thao tác',
            key: 'action',
            render: (_, record) => (
                <Space size="middle">
                    <Popconfirm
                        title="Xóa khuyến mại"
                        description="Bạn có chắc là muốn xóa khuyến mại?"
                        onConfirm={() => confirmRemove(record.id)}
                        okText="Chắc chắn"
                        cancelText="Hủy bỏ"
                    >
                        <Button icon={<DeleteOutlined />} danger type="primary" size="small">Xóa</Button>
                    </Popconfirm>
                    <Button onClick={() => {
                        setPromotion(record)
                        editshowModal()
                    }} icon={<EditOutlined />} danger type="dashed" size="small">Sửa</Button>
                </Space>
            ),
        },
    ];


    return <div className="bg-white shadow-sm rounded-lg p-6">
        <Table
            columns={columns}
            dataSource={promotions}
            rowKey='id'
            title={() => <TitleHeader onChange={value => {
                setQueryParams({
                    ...queryParams,
                    searchString: value,
                    pageIndex: 1
                })
            }} value={queryParams.searchString} title="Khuyến mại" callback={showModal} />}
            pagination={{
                pageSize: queryParams.pageSize,
                current: queryParams.pageIndex,
                onChange: (value) => {
                    const updateParams = {
                        ...queryParams,
                        pageIndex: value,
                    }
                    setQueryParams(updateParams)
                    fetchPromotions(updateParams)
                },
                align: 'end',
                showLessItems: true,
                total: total
            }}
        />

        <Modal
            open={isModalOpen}
            onOk={handleOk}
            title={<p className="text-center font-semibold text-2xl">Thêm khuyến mại mới</p>}
            onCancel={handleCancel}
            footer={[]}
        >
            <CreatePromotionModal handleOk={handleSubmit} />
        </Modal>

        <Modal
            open={isEditModalOpen}
            onOk={edithandleOk}
            title={<p className="text-center font-semibold text-2xl">Cập nhật khuyến mại</p>}
            onCancel={edithandleCancel}
            footer={[]}
        >
            {/* <EditBrandModal brand={brand!} handleOk={handleSubmit} /> */}
        </Modal>
    </div>
};

export default PromotionManagement;