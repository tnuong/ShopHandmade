import { FC } from "react";
import { Link } from "react-router-dom";

const Logo: FC = () => {
    return (
        <Link to='/' className="p-3 flex justify-center items-center rounded-full font-bold bg-transparent text-xl md:text-2xl lg:text-3xl">
            <div className="text-3xl font-bold">
                HAND<span className="text-red-500">MADE</span>
            </div>
        </Link>
    );
};

export default Logo;
