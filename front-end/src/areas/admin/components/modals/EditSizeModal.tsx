import { Button, Form, FormProps, Input, message } from "antd";
import { FC, useEffect } from "react";
import { SizeResource } from "../../../../resources";
import { SizeRequest } from "./CreateSizeModal";
import sizeService from "../../../../services/size-service";


type EditSizeModalProps = {
    handleOk: () => void;
    size: SizeResource;
}

const EditSizeModal: FC<EditSizeModalProps> = ({
    handleOk,
    size
}) => {

    useEffect(() => {
        if (size) {
            form.setFieldsValue({
                eSize: size?.eSize,
                description: size?.description,
            });
        }
    }, [size]);

    const [form] = Form.useForm<SizeRequest>();

    const onFinish: FormProps<SizeRequest>['onFinish'] = async (values): Promise<void> => {
        await sizeService.updateSize(size.id, values);
        message.success('Cập nhật kích cỡ thành công')
        form.resetFields();
        handleOk()
    };

    const onFinishFailed: FormProps<SizeRequest>['onFinishFailed'] = (errorInfo) => {
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
            <Form.Item<SizeRequest>
                label="Tên kích cỡ"
                name="eSize"
                rules={[{ required: true, message: 'Tên kích cỡ không được để trống!' }]}
            >
                <Input size="large" placeholder="Tên kích cỡ ..." />
            </Form.Item>


            <Form.Item<SizeRequest>
                label="Mô tả"
                name="description"
                rules={[{ required: true, message: 'Mô tả không được để trống!' }]}
            >
                <Input.TextArea size="large" placeholder="Mô tả ..." />
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

export default EditSizeModal;