import { FC, useEffect, useState } from "react";
import { Button, Modal, Popconfirm, Space, Table, message } from 'antd';
import type { TableProps } from 'antd';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons';
import useModal from "../../../hooks/useModal";
import TitleHeader from "../components/TitleHeader";
import { QueryParams } from "./products/VariantManagement";
import useDebounce from "../../../hooks/useDebounce";
import { SupplierResource } from "../../../resources";
import supplierService from "../../../services/supplier-service";
import CreateSupplierModal from "../components/modals/CreateSupplierModal";
import EditSupplierModal from "../components/modals/EditSupplierModal";

const intitialValues: QueryParams = {
    pageIndex: 1,
    pageSize: 8,
    searchString: ''
}

const SupplierManagement: FC = () => {
    const { isModalOpen, handleCancel, handleOk, showModal } = useModal()
    const { isModalOpen: isEditModalOpen, handleCancel: edithandleCancel, handleOk: edithandleOk, showModal: editshowModal } = useModal()
    const [supplier, setSupplier] = useState<SupplierResource | null>(null)
    const [suppliers, setSuppliers] = useState<SupplierResource[]>([]);
    const [queryParams, setQueryParams] = useState<QueryParams>(intitialValues)
    const [total, setTotal] = useState(8)
    const debounceValue = useDebounce(queryParams.searchString, 600)

    const fetchSuppliers = async (params: QueryParams) => {
        const response = await supplierService.getAllSuppliers(params);
        setSuppliers(response.data)
        setTotal(response.pagination.totalItems)
    }

    const confirmRemove = async (id: number) => {
        const response = await supplierService.removeSupplier(id)

        if(response.success) {
            message.success(response.message);
            fetchSuppliers(queryParams)
        }
        else {
            message.error(response.message);
        }
      
    };

    const handleSubmit = () => {
        edithandleOk();
        handleOk();
        fetchSuppliers(queryParams)
    }


    useEffect(() => {
        fetchSuppliers(queryParams)
    }, [debounceValue]);


    const columns: TableProps<SupplierResource>['columns'] = [
        {
            title: 'Nhà cung cấp',
            dataIndex: 'name',
            key: 'name',
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
            title: 'Trạng thái',
            dataIndex: 'status',
            key: 'status',
            render: (value) => {
                if(value) {
                    return <span className="text-green-500 font-semibold">Hoạt động</span>
                }
                return <span className="text-red-500 font-semibold">Ngừng hoạt động</span>
            }
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
                        title="Xóa nhà cung cấp"
                        description="Bạn có chắc là muốn xóa nhà cung cấp?"
                        onConfirm={() => confirmRemove(record.id)}
                        okText="Chắc chắn"
                        cancelText="Hủy bỏ"
                    >
                        <Button icon={<DeleteOutlined />} danger type="primary" size="small">Xóa</Button>
                    </Popconfirm>
                    <Button onClick={() => {
                        setSupplier(record)
                        editshowModal()
                    }} icon={<EditOutlined />} danger type="dashed" size="small">Sửa</Button>
                </Space>
            ),
        },
    ];


    return <div className="bg-white shadow-sm rounded-lg p-6">
        <Table
            columns={columns}
            dataSource={suppliers}
            rowKey='id'
            title={() => <TitleHeader onChange={value => {
                setQueryParams({
                    ...queryParams,
                    searchString: value,
                    pageIndex: 1
                })
            }} value={queryParams.searchString} title="Nhà cung cấp" callback={showModal} />}
            pagination={{
                pageSize: queryParams.pageSize,
                current: queryParams.pageIndex,
                onChange: (value) => {
                    const updateParams = {
                        ...queryParams,
                        pageIndex: value,
                    }
                    setQueryParams(updateParams)
                    fetchSuppliers(updateParams)
                },
                align: 'end',
                showLessItems: true,
                total: total
            }}
        />

        <Modal
            open={isModalOpen}
            onOk={handleOk}
            title={<p className="text-center font-semibold text-2xl">Thêm nhà cung cấp mới</p>}
            onCancel={handleCancel}
            footer={[]}
        >
            <CreateSupplierModal handleOk={handleSubmit} />
        </Modal>

        <Modal
            open={isEditModalOpen}
            onOk={edithandleOk}
            title={<p className="text-center font-semibold text-2xl">Cập nhật nhà cung cấp</p>}
            onCancel={edithandleCancel}
            footer={[]}
        >
            <EditSupplierModal supplier={supplier!} handleOk={handleSubmit} />
        </Modal>
    </div>
};

export default SupplierManagement;