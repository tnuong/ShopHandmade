import { FC } from "react";

import NavbarRight from "./NavbarRight";
import { Link } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars } from "@fortawesome/free-solid-svg-icons";
import Logo from "../../areas/shared/Logo";


type HeaderFadeInProps = {
    fixed: boolean;
}

const HeaderFadeIn: FC<HeaderFadeInProps> = ({
    fixed
}) => {

    return <div className={`${fixed && 'translate-y-24'} shadow-sm bg-white fixed -top-24 left-0 right-0 z-20 transition-all ease-linear flex items-center justify-between px-10 h-24`}>
        <FontAwesomeIcon className="block lg:hidden text-2xl" icon={faBars} />
        <Logo />
        <div className="lg:flex gap-x-8 hidden text-black font-semibold">
            <Link to="/" className="text-lg">Trang chủ</Link>
            <Link to="/shop" className="text-lg">Cửa hàng</Link>
            <Link to="/blog" className="text-lg">Bài viết</Link>
            <Link to='/contact' className="text-lg">Liên hệ</Link>
        </div>
        <NavbarRight />
    </div>;
};

export default HeaderFadeIn;

