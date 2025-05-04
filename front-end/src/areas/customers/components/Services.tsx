import { faArrowsRotate, faMessage, faTruck } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { FC } from "react";

const Services: FC = () => {
    return <div className="grid grid-cols-1 gap-8 lg:grid-cols-3 lg:gap-0">
        <div className="flex flex-col items-center gap-y-2 bg-slate-50 py-10 px-4">
            <FontAwesomeIcon className="text-xl" icon={faTruck} />
            <span className="font-semibold text-2xl">Free shipping</span>
            <p className="text-center text-gray-500 text-lg">We are delighted to inform you that your recent order qualifies for our free shipping promotion!</p>
        </div>
        <div className="flex flex-col items-center gap-y-2 bg-slate-50 py-10 px-4">
            <FontAwesomeIcon className="text-xl" icon={faArrowsRotate} />
            <span className="font-semibold text-2xl">Free Returns</span>
            <p className="text-center text-gray-500 text-lg">Free return within 10 days, please make sure the items are in undamaged condition</p>
        </div>
        <div className="flex flex-col items-center gap-y-2 bg-slate-50 py-10 px-4">
            <FontAwesomeIcon className="text-xl" icon={faMessage} />
            <span className="font-semibold text-2xl">Free shipping</span>
            <p className="text-center text-gray-500 text-lg">We support customer 24/7, Send us your questions, and we will respond quickly.</p>
        </div>
    </div>
};

export default Services;
