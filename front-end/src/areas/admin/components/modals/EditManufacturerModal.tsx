import { Button, Form, FormProps, Input, message } from "antd";
import { FC, useEffect } from "react";
import { ManufacturerResource } from "../../../../resources";
import { ManufacturerRequest } from "./CreateManufacturerModal";
import manufacturerService from "../../../../services/manufacturer-service";


type EditManufacturerModalProps = {
    handleOk: () => void;
    manufacturer: ManufacturerResource;
}

const EditManufacturerModal: FC<EditManufacturerModalProps> = ({
    handleOk,
    manufacturer
}) => {

    useEffect(() => {
        if (manufacturer) {
            form.setFieldsValue({
                name: manufacturer?.name,
                description: manufacturer?.description,
                address: manufacturer?.address,
                phoneNumber: manufacturer?.phoneNumber,
                email: manufacturer?.email,
            });
        }
    }, [manufacturer]);

    const [form] = Form.useForm<ManufacturerRequest>(); 
    
    const onFinish: FormProps<ManufacturerRequest>['onFinish'] = async (values) : Promise<void> => {
        await manufacturerService.updateManufacturer(manufacturer.id,  values);
        message.success('Cập nhật nhà sản xuất thành công')
        form.resetFields();
        handleOk()
    };

    const onFinishFailed: FormProps<ManufacturerRequest>['onFinishFailed'] = (errorInfo) => {
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
            <Form.Item<ManufacturerRequest>
                label="Tên nhà sản xuất"
                name="name"
                rules={[{ required: true, message: 'Tên nhà sản xuất không được để trống!' }]}
            >
                <Input size="large" placeholder="Tên nhà sản xuất ..." />
            </Form.Item>

            <Form.Item<ManufacturerRequest>
                label="Mô tả"
                name="description"
                rules={[{ required: true, message: 'Mô tả nhà sản xuất không được để trống!' }]}
            >
                <Input.TextArea size="large" placeholder="Mô tả ..." />
            </Form.Item>

            <Form.Item<ManufacturerRequest>
                label="Số điện thoại"
                name="phoneNumber"
                rules={[
                    { required: true, message: 'Số điện thoại không được để trống!' },
                    { pattern: /^\d{10}$/, message: 'Số điện thoại phải gồm 10 chữ số!' },
                ]}
            >
                <Input size="large" placeholder="Số điện thoại ..." />
            </Form.Item>

            <Form.Item<ManufacturerRequest>
                label="Địa chỉ email"
                name="email"
                rules={[
                    { required: true, message: 'Địa chỉ email không được để trống!' },
                    { type: 'email', message: 'Địa chỉ email không hợp lệ!' },
                ]}
            >
                <Input size="large" placeholder="Địa chỉ email ..." />
            </Form.Item>

            <Form.Item<ManufacturerRequest>
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

export default EditManufacturerModal;