import { Button, Form, FormProps, Input, message, Switch } from "antd";
import { FC, useEffect } from "react";
import { SupplierResource } from "../../../../resources";
import supplierService from "../../../../services/supplier-service";
import { SupplierRequest } from "./CreateSupplierModal";


type EditSupplierModalProps = {
    handleOk: () => void;
    supplier: SupplierResource;
}

const EditSupplierModal: FC<EditSupplierModalProps> = ({
    handleOk,
    supplier
}) => {

    useEffect(() => {
        if (supplier) {
            form.setFieldsValue({
                name: supplier?.name,
                status: supplier?.status,
                address: supplier?.address,
                phoneNumber: supplier?.phoneNumber,
                email: supplier?.email,
            });
        }
    }, [supplier]);

    const [form] = Form.useForm<SupplierRequest>(); 
    
    const onFinish: FormProps<SupplierRequest>['onFinish'] = async (values) : Promise<void> => {
        await supplierService.updateSupplier(supplier.id,  values);
        message.success('Cập nhật nhà cung cấp thành công')
        form.resetFields();
        handleOk()
    };

    const onFinishFailed: FormProps<SupplierRequest>['onFinishFailed'] = (errorInfo) => {
        console.log('Failed:', errorInfo);
    };

    return <div className="px-4 pt-4 max-h-[500px] overflow-y-auto custom-scrollbar scrollbar-h-4">
        <Form
            form={form}
            name="basic"
            onFinish={onFinish}
            layout="vertical"
            onFinishFailed={onFinishFailed}
            autoComplete="off"
        >
            <Form.Item<SupplierRequest>
                label="Tên nhà cung cấp"
                name="name"
                rules={[{ required: true, message: 'Tên nhà cung cấp không được để trống!' }]}
            >
                <Input size="large" placeholder="Tên nhà cung cấp ..." />
            </Form.Item>

            <Form.Item<SupplierRequest>
                label="Trạng thái"
                name="status"
                valuePropName="checked" 
                rules={[{ required: true, message: 'Trạng thái nhà cung cấp không được để trống!' }]}
            >
                <Switch defaultChecked />
            </Form.Item>

            <Form.Item<SupplierRequest>
                label="Số điện thoại"
                name="phoneNumber"
                rules={[
                    { required: true, message: 'Số điện thoại không được để trống!' },
                    { pattern: /^\d{10}$/, message: 'Số điện thoại phải gồm 10 chữ số!' },
                ]}
            >
                <Input size="large" placeholder="Số điện thoại ..." />
            </Form.Item>

            <Form.Item<SupplierRequest>
                label="Địa chỉ email"
                name="email"
                rules={[
                    { required: true, message: 'Địa chỉ email không được để trống!' },
                    { type: 'email', message: 'Địa chỉ email không hợp lệ!' },
                ]}
            >
                <Input size="large" placeholder="Địa chỉ email ..." />
            </Form.Item>

            <Form.Item<SupplierRequest>
                label="Địa chỉ"
                name="address"
                rules={[{ required: true, message: 'Địa chỉ không được để trống!' }]}
            >
                <Input size="large" placeholder="Địa chỉ ..." />
            </Form.Item>

            <div className="flex justify-end">
                <Form.Item>
                    <Button shape="round" size="large" className="mt-4" type="primary" htmlType="submit">
                        Lưu lại
                    </Button>
                </Form.Item>
            </div>
        </Form>
    </div>
};

export default EditSupplierModal;