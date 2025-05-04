import { Button, Image, Modal, Tag } from "antd";
import { FC } from "react";
import {
    EyeOutlined
} from '@ant-design/icons';
import useModal from "../../../hooks/useModal";
import OrderDetailsModal from "./modals/OrderDetailsModal";
import { OrderResource } from "../../../resources";
import { formatCurrencyVND, formatDateTime } from "../../../utils/format";
import { OrderStatus } from "../../../constants/OrderStatus";

type OrderCardProps = {
    order: OrderResource;
    onCancel: (id: number) => void;
    onCompleted: (id: number) => void;
}

const OrderCard: FC<OrderCardProps> = ({
    order,
    onCancel,
    onCompleted
}) => {
    const { handleCancel, handleOk, isModalOpen, showModal } = useModal();


    return <div className="flex items-center justify-between p-8 py-6 rounded-3xl border-[1px] border-gray-300">
        <div className="flex flex-1 flex-col gap-y-4 w-full">
            <div className="flex gap-x-1 items-start w-full">
                <Tag color="volcano">{order.orderStatus}</Tag> |
                <span>{formatDateTime(new Date(order.createdAt.toString()))}</span>
            </div>
            <div className="flex justify-between items-center">
                <div className="flex gap-x-2 items-center">
                    <Image className="rounded-lg object-cover" height='80px' width='80px' src={order.thumbnailUrl} />
                    <div className="flex text-sm flex-col gap-y-2">
                        <span className="font-semibold text-[16px]">ORDER ID: <b className="text-primary">{order.id}</b></span>
                        <p>{order.title}</p>
                        <p className="text-primary font-semibold">{formatCurrencyVND(order.totalPrice)}</p>
                    </div>
                </div>

            </div>
        </div>

        <div className="flex items-center gap-x-2">
            {order.orderStatus === OrderStatus.PENDING && <Button onClick={() => onCancel(order.id)} size="small" type="primary" danger>
                Hủy đơn
            </Button>}

            {order.orderStatus === OrderStatus.DELIVERED && <Button onClick={() => onCompleted(order.id)}  size="small" type="primary">
                Đã nhận
            </Button>}
            <Button
                size="small"
                onClick={showModal}
                icon={<EyeOutlined />}
            >
                Chi tiết
            </Button>
        </div>

        <Modal
            open={isModalOpen}
            onOk={handleOk}
            onCancel={handleCancel}
            title={<p className="text-center font-semibold text-2xl">CHI TIẾT ĐƠN HÀNG</p>}
            footer={[]}
            width='1000px'
        >
            <OrderDetailsModal orderId={order.id} />
        </Modal>
    </div>
};

export default OrderCard;
