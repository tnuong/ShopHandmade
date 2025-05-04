import { FC, useEffect, useState } from "react";
import { Button, Modal, Popconfirm, Space, Table, message } from 'antd';
import type { TableProps } from 'antd';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons';
import useModal from "../../../hooks/useModal";
import { ManufacturerResource } from "../../../resources";
import TitleHeader from "../components/TitleHeader";
import { QueryParams } from "./products/VariantManagement";
import useDebounce from "../../../hooks/useDebounce";
import manufacturerService from "../../../services/manufacturer-service";
import EditManufacturerModal from "../components/modals/EditManufacturerModal";
import CreateManufacturerModal from "../components/modals/CreateManufacturerModal";

const intitialValues: QueryParams = {
    pageIndex: 1,
    pageSize: 8,
    searchString: ''
}

const ManufacturerManagement: FC = () => {
    const { isModalOpen, handleCancel, handleOk, showModal } = useModal()
    const { isModalOpen: isEditModalOpen, handleCancel: edithandleCancel, handleOk: edithandleOk, showModal: editshowModal } = useModal()
    const [manufacturer, setManufacturer] = useState<ManufacturerResource | null>(null)
    const [manufacturers, setManufacturers] = useState<ManufacturerResource[]>([]);
    const [queryParams, setQueryParams] = useState<QueryParams>(intitialValues)
    const [total, setTotal] = useState(8)
    const debounceValue = useDebounce(queryParams.searchString, 600)

    const fetchManufacturers = async (params: QueryParams) => {
        const response = await manufacturerService.getAllManufacturers(params);
        setManufacturers(response.data)
        setTotal(response.pagination.totalItems)
    }

    const confirmRemove = async (id: number) => {
        const response = await manufacturerService.removeManufacturer(id);

        if (response.success) {
            fetchManufacturers(queryParams)
            message.success(response.message);
        } else {
            message.error(response.message);

        }

    };

    const handleSubmit = () => {
        edithandleOk();
        handleOk();
        fetchManufacturers(queryParams)
    }


    useEffect(() => {
        fetchManufacturers(queryParams)
    }, [debounceValue]);


    const columns: TableProps<ManufacturerResource>['columns'] = [
        {
            title: 'Nhà sản xuất',
            dataIndex: 'name',
            key: 'name',
        },
        {
            title: 'Mô tả',
            dataIndex: 'description',
            key: 'description',
        },
        {
            title: 'Email',
            dataIndex: 'email',
            key: 'email',
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber',
            key: 'phoneNumber',
        },
        {
            title: 'Địa chỉ',
            dataIndex: 'address',
            key: 'address',
        },
        {
            title: 'Thao tác',
            key: 'action',
            render: (_, record) => (
                <Space size="middle">
                    <Popconfirm
                        title="Xóa nhà sản xuất"
                        description="Bạn có chắc là muốn xóa nhà sản xuất?"
                        onConfirm={() => confirmRemove(record.id)}
                        okText="Chắc chắn"
                        cancelText="Hủy bỏ"
                    >
                        <Button icon={<DeleteOutlined />} danger type="primary" size="small">Xóa</Button>
                    </Popconfirm>
                    <Button onClick={() => {
                        setManufacturer(record)
                        editshowModal()
                    }} icon={<EditOutlined />} danger type="dashed" size="small">Sửa</Button>
                </Space>
            ),
        },
    ];


    return <div className="bg-white shadow-sm rounded-lg p-6">
        <Table
            columns={columns}
            dataSource={manufacturers}
            rowKey='id'
            title={() => <TitleHeader onChange={value => {
                setQueryParams({
                    ...queryParams,
                    searchString: value,
                    pageIndex: 1
                })
            }} value={queryParams.searchString} title="Nhà sản xuất" callback={showModal} />}
            pagination={{
                pageSize: queryParams.pageSize,
                current: queryParams.pageIndex,
                onChange: (value) => {
                    const updateParams = {
                        ...queryParams,
                        pageIndex: value,
                    }
                    setQueryParams(updateParams)
                    fetchManufacturers(updateParams)
                },
                align: 'end',
                showLessItems: true,
                total: total
            }}
        />

        <Modal
            open={isModalOpen}
            onOk={handleOk}
            title={<p className="text-center font-semibold text-2xl">Thêm nhà sản xuất mới</p>}
            onCancel={handleCancel}
            footer={[]}
        >
            <CreateManufacturerModal handleOk={handleSubmit} />
        </Modal>

        <Modal
            open={isEditModalOpen}
            onOk={edithandleOk}
            title={<p className="text-center font-semibold text-2xl">Cập nhật nhà sản xuất</p>}
            onCancel={edithandleCancel}
            footer={[]}
        >
            <EditManufacturerModal manufacturer={manufacturer!} handleOk={handleSubmit} />
        </Modal>
    </div>
};

export default ManufacturerManagement;