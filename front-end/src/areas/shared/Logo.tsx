import { FC } from "react";
import { Link } from "react-router-dom";

const Logo: FC = () => {
    return (
        <Link to='/' className="p-3 flex justify-center items-center rounded-full font-bold bg-transparent text-xl md:text-2xl lg:text-3xl">
            <span className="text-black">&lt;Hand</span><span className="text-primary">Made /&gt;</span>
        </Link>
    );
};

export default Logo;
