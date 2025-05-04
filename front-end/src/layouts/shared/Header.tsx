import { FC } from "react";
import Logo from "../../areas/shared/Logo";
import { Link } from "react-router-dom";
import NavbarRight from "./NavbarRight";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars } from "@fortawesome/free-solid-svg-icons";

const Header: FC = () => { 
    return <div className={`flex items-center justify-between bg-white px-10 shadow-sm min-h-24`}>
        <FontAwesomeIcon className="block lg:hidden text-2xl" icon={faBars} />
        <Logo />
        <div className="lg:flex gap-x-8 hidden text-black font-semibold">
            <Link to="/" className="text-lg">Trang chủ</Link>
            <Link to="/shop" className="text-lg">Cửa hàng</Link>
            <Link to="/blog" className="text-lg">Bài viết</Link>
            <Link to='/contact' className="text-lg">Liên hệ</Link>
        </div>
        <NavbarRight />
        
    </div>
};

export default Header;
