import { FC, useState } from "react";
import { OrderStatus, OrderStatusType } from "../../../constants/OrderStatus";

const statuses: OrderStatusType[] = [
    OrderStatus.ALL,
    OrderStatus.PENDING,
    OrderStatus.CANCELLED,
    OrderStatus.REJECTED,
    OrderStatus.CONFIRMED,
    OrderStatus.DELIVERING,
    OrderStatus.DELIVERED,
    OrderStatus.COMPLETED
];

type OrderStatusGroupProps = {
    onChange: (value: OrderStatusType) => void
}

const OrderStatusGroup: FC<OrderStatusGroupProps> = ({
    onChange
}) => {
    const [checked, setChecked] = useState<OrderStatusType>(OrderStatus.ALL);

    const handleChange = (status: OrderStatusType) => {
        onChange(status)
        setChecked(status)
    }

    return (
        <div className="flex gap-2 lg:gap-x-4 my-4 lg:my-6 flex-wrap">
            {statuses.map(status => (
                <div
                    key={status}
                    onClick={() => handleChange(status)}
                    className={`px-2 lg:px-3 lg:py-1 cursor-pointer rounded-3xl border-[1px] ${checked === status ? 'border-primary text-primary' : 'border-gray-300 text-gray-700'}`}
                >
                    {status}
                </div>
            ))}
        </div>
    );
};

export default OrderStatusGroup;