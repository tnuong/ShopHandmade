import { Button, Divider, Form, FormProps, Input, message, Select } from "antd";
import { FC, useEffect, useState } from "react";
import brandService from "../../../../services/brand-service";
import { PromotionResource } from "../../../../resources";
import promotionService from "../../../../services/promotion-service";
import { PromotionType } from "../../../../constants/PromotionType";
import { formatCurrencyVND } from "../../../../utils/format";



type AssignDiscountModalProps = {
    onSubmit: (selectedPromotions: PromotionResource[]) => void;
}

const AssignDiscountModal: FC<AssignDiscountModalProps> = ({
    onSubmit
}) => {
    const [promotions, setPromotions] = useState<PromotionResource[]>([]);
    const [selectedPromotions, setSelectedPromotions] = useState<PromotionResource[]>([]);

    const fetchPromotions = async () => {
        const response = await promotionService.getAllPromotionsNonPagination();

        if (response.success) {
            setPromotions(response.data)
        }
    }

    const handleChange = (value: number[]) => {
        const values = promotions.filter(p => value.includes(p.id));
        setSelectedPromotions(values)
    };

    useEffect(() => {
        fetchPromotions()
    }, [])

    const handleSave = () => {
        onSubmit?.(selectedPromotions)
    }

    return <div className="px-4 pt-4 max-h-[500px] overflow-y-auto custom-scrollbar scrollbar-h-4">

        <div className="grid grid-cols-2 gap-4">
            <div className="p-2 flex flex-col gap-y-3">
                <span className="font-semibold">Chọn mã khuyến mãi để áp dụng</span>
                <Select
                    mode="multiple"
                    allowClear
                    style={{ width: '100%' }}
                    placeholder="Chọn mã khuyến mãi"
                    onChange={handleChange}
                    options={promotions.map(p => ({
                        value: p.id,
                        label: p.name
                    }))}
                />
            </div>
            <div className="p-2 flex flex-col gap-y-3">
                <span className="font-semibold">Các khuyến mãi đã chọn</span>

                <div className="flex flex-col gap-2">
                    {selectedPromotions.map(promotion => <div key={promotion.id}>
                        <span>{promotion.name}</span> |
                        Giảm <span>{promotion.promotionType === PromotionType.FIXED_AMOUNT ? formatCurrencyVND(promotion.discountValue) : `${promotion.discountValue}%`}</span>
                    </div>)}
                </div>
            </div>
        </div>

        <Divider />

        <Button onClick={handleSave} type="primary">LƯU LẠI</Button>
    </div>
};

export default AssignDiscountModal;