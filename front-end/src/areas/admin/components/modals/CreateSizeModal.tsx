import { Button, Form, FormProps, Input, InputNumber, message } from "antd";
import { FC } from "react";
import sizeService from "../../../../services/size-service";


export type SizeRequest = {
    eSize: string;
    description: string;
};

type CreateSizeModalProps = {
    handleOk: () => void;
}

const CreateSizeModal: FC<CreateSizeModalProps> = ({
    handleOk
}) => {
    const [form] = Form.useForm<SizeRequest>();

    const onFinish: FormProps<SizeRequest>['onFinish'] = async (values) => {
        const response = await sizeService.createSize(values);
        message.success(response.message)
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

export default CreateSizeModal;