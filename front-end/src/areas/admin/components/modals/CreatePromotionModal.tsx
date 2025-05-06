import { Button, Form, FormProps, Input, message, Select, DatePicker, Switch } from "antd";
import { FC } from "react";
import promotionService from "../../../../services/promotion-service";
import { PromotionType } from "../../../../constants/PromotionType";
import dayjs from "dayjs";

export type PromotionRequest = {
    name?: string;
    description?: string;
    promotionType?: string;
    isActive?: boolean;
    discountValue?: string;
    startDate?: Date;
    endDate?: Date;
};

type CreatePromotionModalProps = {
    handleOk: () => void;
};

const CreatePromotionModal: FC<CreatePromotionModalProps> = ({ handleOk }) => {
    const [form] = Form.useForm<PromotionRequest>();

    const onFinish: FormProps<PromotionRequest>['onFinish'] = async (values) => {
        const response = await promotionService.createPromotion({
            ...values,
            startDate: values.startDate,
            endDate: values.endDate,
        });

        if (response.success) {
            message.success(response.message);
            form.resetFields();
            handleOk();
        } else {
            message.error(response.message);
        }
    };

    const onFinishFailed: FormProps<PromotionRequest>['onFinishFailed'] = (errorInfo) => {
        console.log('Failed:', errorInfo);
    };

    return (
        <div className="px-4 pt-4 max-h-[500px] overflow-y-auto custom-scrollbar scrollbar-h-4">
            <Form
                form={form}
                name="basic"
                onFinish={onFinish}
                layout="vertical"
                onFinishFailed={onFinishFailed}
                autoComplete="off"
                initialValues={{
                    isActive: true
                }}
            >
                <Form.Item<PromotionRequest>
                    label="Tên khuyến mại"
                    name="name"
                    rules={[{ required: true, message: 'Tên khuyến mại không được để trống!' }]}
                >
                    <Input size="large" placeholder="Tên khuyến mại ..." />
                </Form.Item>

                <Form.Item<PromotionRequest>
                    label="Nội dung"
                    name="description"
                    rules={[{ required: true, message: 'Nội dung không được để trống!' }]}
                >
                    <Input.TextArea size="large" placeholder="Nội dung ..." />
                </Form.Item>

                <Form.Item<PromotionRequest>
                    label="Loại khuyến mại"
                    name="promotionType"
                >
                    <Select placeholder='Chọn loại khuyến mại' size="large">
                        <Select.Option value={PromotionType.FIXED_AMOUNT}>Số tiền cố định</Select.Option>
                        <Select.Option value={PromotionType.PERCENTAGE}>Phần trăm</Select.Option>
                    </Select>
                </Form.Item>

                <Form.Item<PromotionRequest>
                    label="Giá trị giảm"
                    name="discountValue"
                    rules={[{ required: true, message: 'Gía trị giảm không được để trống!' }]}
                >
                    <Input type="number" size="large" placeholder="Gía trị giảm ..." />
                </Form.Item>

                <Form.Item<PromotionRequest>
                    label="Từ ngày"
                    name="startDate"
                    rules={[{ required: true, message: 'Vui lòng chọn ngày bắt đầu' }]}
                >
                    <DatePicker size="large" className="w-full" disabledDate={(current) => current && current < dayjs().startOf('day')} />
                </Form.Item>

                <Form.Item<PromotionRequest>
                    label="Đến ngày"
                    name="endDate"
                    rules={[
                        { required: true, message: 'Vui lòng chọn ngày kết thúc' },
                        ({ getFieldValue }) => ({
                            validator(_, value) {
                                const fromDate = getFieldValue('fromDate');
                                if (!value || !fromDate) return Promise.resolve();
                                if (value.isSameOrBefore(fromDate)) {
                                    return Promise.reject(new Error('Ngày kết thúc phải lớn hơn ngày bắt đầu'));
                                }
                                return Promise.resolve();
                            },
                        }),
                    ]}
                >
                    <DatePicker size="large" className="w-full" />
                </Form.Item>

                <Form.Item<PromotionRequest>
                    label="Kích hoạt"
                    name="isActive"
                    valuePropName="checked"
                >
                    <Switch />
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
    );
};

export default CreatePromotionModal;
