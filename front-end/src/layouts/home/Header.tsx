import { useState } from "react";
import { FaUser, FaHeart, FaShoppingCart, FaBars } from "react-icons/fa";
import { Drawer, Modal, Button } from "antd";
import { Link } from "react-router-dom";

const Header = () => {
    const [isDrawerOpen, setIsDrawerOpen] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [authMode, setAuthMode] = useState<'login' | 'register'>('login');

    const toggleDrawer = () => setIsDrawerOpen(!isDrawerOpen);
    const openModal = (mode: 'login' | 'register') => {
        setAuthMode(mode);
        setIsModalOpen(true);
    };

    return (
        <header className="sticky top-0 w-full z-50">
            {/* Topbar */}
            {/* <div className="bg-[#14131A] text-white text-sm px-4 py-2 flex justify-between items-center">
                <span className="flex items-center gap-1">
                    🌟 MIỄN PHÍ VẬN CHUYỂN TẠI ĐÀ NẴNG CHO ĐƠN HÀNG TỪ 100.000VNĐ 🌟
                </span>
                <div className="flex gap-4">
                    <span>MUA HÀNG NGAY ĐI NÀO 🌟</span>

                </div>
            </div> */}

            {/* Main Header */}
            <div className="bg-white flex justify-between items-center px-6 py-4 shadow-md relative">
                {/* Hamburger menu for mobile */}
                <FaBars
                    className="text-xl md:hidden cursor-pointer"
                    onClick={toggleDrawer}
                />

                {/* Logo */}
                <div className="text-3xl font-bold">
                    HAND<span className="text-red-500">MADE</span>
                </div>

                {/* Navbar */}
                <nav className="hidden md:flex gap-8 text-lg font-semibold text-black">
                    <Link to="/">TRANG CHỦ</Link>
                    <Link to="/shop">CỬA HÀNG</Link>
                    <Link to="/">KHUYẾN MÃI</Link>
                    <Link to="/account/wishlist">YÊU THÍCH</Link>
                    <Link to="/blog">BLOG</Link>
                    <Link to="/">LIÊN HỆ</Link>
                </nav>

                {/* Icons */}
                <div className="flex items-center gap-6 text-black text-lg relative">
                    <FaUser className="cursor-pointer" onClick={() => openModal('login')} />
                    <FaHeart className="cursor-pointer" />
                    <div className="relative cursor-pointer">
                        <FaShoppingCart />
                        <span className="absolute -top-2 -right-2 bg-red-500 text-white text-xs w-5 h-5 rounded-full flex items-center justify-center">
                            2
                        </span>
                    </div>
                </div>
            </div>

            {/* Drawer Menu (Mobile) */}
            <Drawer
                title="Menu"
                placement="left"
                onClose={toggleDrawer}
                open={isDrawerOpen}
            >

            </Drawer>

            {/* Auth Modal */}
            <Modal
                open={isModalOpen}
                onCancel={() => setIsModalOpen(false)}
                footer={null}
                centered
                width={authMode === 'register' ? 600 : 400}
            >
                <div className="p-4">
                    <h2 className="text-xl font-semibold mb-4">
                        {authMode === 'login' ? 'Đăng nhập' : 'Đăng ký'}
                    </h2>
                    {/* Đây là phần form mô phỏng, bạn có thể thay bằng AuthModal thật */}
                    <input
                        type="text"
                        placeholder="Email"
                        className="w-full p-2 border mb-2 rounded"
                    />
                    <input
                        type="password"
                        placeholder="Mật khẩu"
                        className="w-full p-2 border mb-2 rounded"
                    />
                    <Button type="primary" block>
                        {authMode === 'login' ? 'Đăng nhập' : 'Đăng ký'}
                    </Button>
                </div>
            </Modal>
        </header>
    );
};

export default Header;
