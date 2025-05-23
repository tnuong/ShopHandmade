import { FC, useState } from "react";
import HeaderFadeIn from "../shared/HeaderFadeIn";
import Header from "../shared/Header";
import { Outlet } from "react-router-dom";
import Footer from "../shared/Footer";

const DiscountLayout: FC = () => {
    const [fixed, setFixed] = useState(false)


    const handleScroll = (event: React.UIEvent<HTMLDivElement>) => {
        const scrollTop = event.currentTarget.scrollTop;
        if (scrollTop >= 96) {
            setFixed(true)
        } else if (scrollTop === 0) {
            setFixed(false)
        }
    }

    return <div onScroll={handleScroll} className="relative h-screen overflow-y-auto bg-slate-50">
        {/* <HeaderFadeIn fixed={fixed} /> */}
        <Header />

        <div className="w-full max-w-screen-lg mx-auto">
            <Outlet />
        </div>
        <Footer />
    </div>
};

export default DiscountLayout
