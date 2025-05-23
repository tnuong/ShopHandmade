import { FC } from "react";
import { Link } from "react-router-dom";

const ShopNow: FC = () => {
    return <div className="grid grid-cols-1 md:grid-cols-2" style={{
        height: '80vh'
    }}>
        <div>
            <iframe
                width="100%"
                height="100%"
                className="object-cover"
                title="My video"
                src="https://www.youtube.com/embed/5BXZAkkeDV4"
            >
            </iframe>
        </div>
        <div className="p-12 flex flex-col gap-y-6 justify-center items-start bg-slate-100">
            <p className="font-semibold text-5xl">HANDMADE</p>
            <p className="text-[17px]">Chọn giao hàng vào ngày hôm sau và thêm lời nhắn quà tặng cá nhân khi thanh toán. Đội ngũ studio của chúng tôi sẽ viết tay lời nhắn của bạn lên thiệp quà tặng xinh đẹp, tạo nên một điểm nhấn đặc biệt!</p>
            <Link to='/shop'>
                <button className="text-xl px-4 py-2 rounded-lg bg-orange-400 text-white">SHOP NOW</button>
            </Link>
        </div>
    </div>
};

export default ShopNow;
