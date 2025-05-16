import { FC } from "react";
import { UserOutlined, OrderedListOutlined } from '@ant-design/icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faLocationDot } from "@fortawesome/free-solid-svg-icons";
import { faHeart } from "@fortawesome/free-regular-svg-icons";
import { Link } from "react-router-dom";
import { Tooltip } from "antd";

const Sidebar: FC = () => {
    return <div className="md:p-4 p-2 lg:w-[300px] flex flex-row md:flex-col justify-between md:justify-start lg:flex-col gap-y-3 bg-orange-50 md:bg-transparent top-[90px] z-20 sticky md:static">
        <Link to='/account/profile' className="flex items-center gap-x-3 md:px-4 text-[16px] cursor-pointer p-4 md:p-2 text-primary md:text-black rounded-xl hover:text-gray-800 hover:bg-primary md:hover:bg-gray-100">
            <UserOutlined className="hidden md:block" />
            <Tooltip title='Hồ sơ' className="md:hidden">
                <UserOutlined />
            </Tooltip>
            <span className="hidden md:block">Hồ sơ</span>
        </Link>
        <Link to='/account/wishlist' className="flex items-center gap-x-3 md:px-4 text-[16px] cursor-pointer p-4 md:p-2 text-primary md:text-black rounded-xl hover:text-gray-800 hover:bg-primary md:hover:bg-gray-100">
            <FontAwesomeIcon className="hidden md:block" icon={faHeart} />
            <Tooltip title='Wishlist' className="md:hidden">
                <FontAwesomeIcon icon={faHeart} />
            </Tooltip>
            <span className="hidden md:block">Wishlist</span>
        </Link>
        <Link to='/account/orders' className="flex items-center gap-x-3 md:px-4 text-[16px] cursor-pointer p-4 md:p-2 text-primary md:text-black rounded-xl hover:text-gray-800 hover:bg-primary md:hover:bg-gray-100">
            <OrderedListOutlined className="hidden md:block" />
            <Tooltip title='Đơn hàng của tôi' className="md:hidden">
                <OrderedListOutlined />
            </Tooltip>
            <span className="hidden md:block">Đơn hàng của tôi</span>
        </Link>
        <Link to='/account/address-order' className="flex items-center gap-x-3 md:px-4 text-[16px] cursor-pointer p-4 md:p-2 text-primary md:text-black rounded-xl hover:text-gray-800 hover:bg-primary md:hover:bg-gray-100">
            <FontAwesomeIcon className="hidden md:block" icon={faLocationDot} />
            <Tooltip title='Địa chỉ đã lưu' className="md:hidden">
                <FontAwesomeIcon icon={faLocationDot} />
            </Tooltip>
            <span className="hidden md:block">Địa chỉ đã lưu</span>
        </Link>
    </div>;
};

export default Sidebar;
