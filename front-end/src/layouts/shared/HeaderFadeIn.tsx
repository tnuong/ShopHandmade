import { FC } from "react";
import { Link } from "react-router-dom";
import NavbarRight from "./NavbarRight";

type HeaderFadeInProps = {
  fixed: boolean;
};

const HeaderFadeIn: FC<HeaderFadeInProps> = ({ fixed }) => {
  return (
    <div className="fixed top-0 left-0 right-0 z-50">
      {/* Thanh trên cùng - ngôn ngữ, tài khoản */}
      {/* <div className="bg-[#1a1926] text-white text-sm h-10 flex justify-end items-center px-10 gap-6">
    
        <div className="cursor-pointer"></div>
        <div className="flex items-center gap-1 cursor-pointer">
       
        </div>
      </div> */}

      {/* Thông báo khuyến mãi */}
      <div className="bg-[#1a1926] text-white text-sm text-center py-2">
        <span className="text-yellow-400">🌟</span> MIỄN PHÍ VẬN CHUYỂN TẠI ĐÀ NẴNG CHO ĐƠN HÀNG TỪ 100.000VNĐ{" "}
        <span className="text-yellow-400">🌟</span>
      </div>

      {/* Header chính */}
      <div
        className={`${
          fixed ? "translate-y-[80px]" : "-translate-y-[80px]"
        } transition-all ease-linear duration-300 bg-white shadow-sm flex items-center justify-between px-10 h-24`}
      >
        {/* Logo */}
        <div className="text-3xl font-bold">
          <span className="text-[#1a1926]">HAND</span>
          <span className="text-[#ff4e4e]">MADE</span>
        </div>

        {/* Menu chính */}
        <div className="hidden lg:flex gap-x-8 text-black font-semibold text-base">
          <Link to="/" className="hover:text-[#ff4e4e]">TRANG CHỦ</Link>
          <Link to="/shop" className="hover:text-[#ff4e4e]">CỬA HÀNG</Link>
          <Link to="/sale" className="hover:text-[#ff4e4e]">KHUYẾN MÃI</Link>
          <Link to="/pages" className="hover:text-[#ff4e4e]">YÊU THÍCH</Link>
          <Link to="/blog" className="hover:text-[#ff4e4e]">BÀI VIẾT</Link>
          <Link to="/contact" className="hover:text-[#ff4e4e]">LIÊN HỆ</Link>
        </div>

        {/* Phần icon dùng lại NavbarRight */}
        <NavbarRight />
      </div>
    </div>
  );
};

export default HeaderFadeIn;
