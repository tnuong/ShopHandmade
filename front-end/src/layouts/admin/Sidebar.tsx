import { FC, useEffect, useState } from "react";
import { AppstoreOutlined, SettingOutlined, MessageOutlined, DashboardOutlined, BookOutlined } from '@ant-design/icons';
import { Button, Menu, MenuProps } from "antd";
import { Link } from "react-router-dom";
import { useSelector } from "react-redux";
import { selectAuth } from "../../feature/auth/authSlice";

type MenuItem = Required<MenuProps>['items'][number];


export const getMenuItemsByRoles = (roles: string[]): MenuItem[] => {
    const hasRole = (role: string) => roles.includes(role);
    const isAdmin = hasRole("ADMIN");
    const isEmployee = hasRole("EMPLOYEE");

    const items: MenuItem[] = [];

    if (isAdmin) {
        items.push({
            key: "sub1",
            label: <Link to="/admin/dashboard">Dashboard</Link>,
            icon: <DashboardOutlined />,
        });
    }

    if (isAdmin || isEmployee) {
        const children: MenuItem[] = [];

        if (isAdmin || isEmployee) {
            children.push(
                { key: "6", label: <Link to="/admin/product">Sản phẩm</Link> },
                { key: "8", label: <Link to="/admin/order">Đơn hàng</Link> }
            );
        }


        if (isAdmin) {
            children.push(
                { key: "1", label: <Link to="/admin/promotion">Khuyến mại</Link> },
                { key: "2", label: <Link to="/admin/manufacturer">Nhà sản xuất</Link> },
                { key: "3", label: <Link to="/admin/supplier">Nhà cung cấp</Link> },
                { key: "4", label: <Link to="/admin/category">Danh mục</Link> },
                { key: "5", label: <Link to="/admin/brand">Nhãn hiệu</Link> },
                { key: "7", label: <Link to="/admin/purchase">Nhập hàng</Link> },
                { key: "9", label: <Link to="/admin/account">Tài khoản</Link> },
            );
        }

      
        items.push({
            key: "sub2",
            label: "Quản lí",
            icon: <AppstoreOutlined />,
            children,
        });
    }

    if (isAdmin) {
        items.push({
            key: "sub4",
            label: "Cài đặt",
            icon: <SettingOutlined />,
            children: [
                { key: "10", label: <Link to="/admin/color">Màu sắc</Link> },
                { key: "11", label: <Link to="/admin/size">Kích cỡ</Link> },
                { key: "12", label: <Link to="/admin/setting/slide">Slide</Link> },
                { key: "13", label: <Link to="/admin/setting/top-review">Top Review</Link> },
            ],
        });

        items.push({
            key: "sub5",
            label: <Link to="/admin/blog">Bài viết</Link>,
            icon: <BookOutlined />,
        });
    }

    if (isAdmin || isEmployee) {
        items.push({
            key: "sub6",
            label: <Link to="/admin/conservation">Liên hệ</Link>,
            icon: <MessageOutlined />,
        });
    }

    return items;
};

const Sidebar: FC = () => {
    const [current, setCurrent] = useState('1');
    const { user } = useSelector(selectAuth)

    const onClick: MenuProps['onClick'] = (e) => {
        setCurrent(e.key);
    };


    return <div className="min-w-[270px] flex flex-col justify-between h-screen overflow-y-auto custom-scrollbar scrollbar-h-4">
        <div className="flex flex-col p-4 gap-y-8">
            <div className="text-2xl font-bold text-center p-2">
                <span className="text-black">&lt;Hand</span><span className="text-primary">Made /&gt;</span>
            </div>
            <Menu
                onClick={onClick}
                style={{ width: 230, border: 0 }}
                defaultOpenKeys={['sub1']}
                selectedKeys={[current]}
                mode="inline"
                items={getMenuItemsByRoles(user?.roles || [])}
                className="text-[17px]"
            />
        </div>
     
    </div>;
};

export default Sidebar;
