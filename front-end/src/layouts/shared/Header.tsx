import { FC } from "react";
import Logo from "../../areas/shared/Logo";
import { Link } from "react-router-dom";
import NavbarRight from "./NavbarRight";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars } from "@fortawesome/free-solid-svg-icons";

const Header: FC = () => {
    return <div className={`sticky top-0 flex items-center justify-between bg-white px-10 shadow-lg min-h-24 z-50`}>
        <FontAwesomeIcon className="block lg:hidden text-2xl" icon={faBars} />
        <Logo />
        <div className="lg:flex gap-x-8 hidden text-black font-semibold">
            <Link to="/">TRANG CHỦ</Link>
            <Link to="/shop">CỬA HÀNG</Link>
            <Link to="/">KHUYẾN MÃI</Link>
            <Link to="/account/wishlist">YÊU THÍCH</Link>
            <Link to="/blog">BLOG</Link>
            <Link to="/">LIÊN HỆ</Link>
        </div>

        <NavbarRight />

    </div>
};

export default Header;
