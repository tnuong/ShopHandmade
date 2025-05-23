import { FC, useState } from "react";
import Slider from "./Slider";
import { Outlet } from "react-router-dom";
import Footer from "../shared/Footer";
import Header from "../shared/Header";

const HomeLayout: FC = () => {
    const [fixed, setFixed] = useState(false)

    const handleScroll = (event: React.UIEvent<HTMLDivElement>) => {
        const scrollTop = event.currentTarget.scrollTop;
        if (scrollTop >= 96) {
            setFixed(true)
        } else if (scrollTop === 0) {
            setFixed(false)
        }

    }

    return <div onScroll={handleScroll} className="relative flex flex-col h-screen overflow-y-auto">
        {/* <HeaderFadeIn fixed={fixed} /> */}
        <Header />
        <div style={{
            minHeight: '90vh'
        }} className="flex flex-col">
            <Slider />
        </div>
        <div>
            <Outlet />
        </div>
        <Footer />
    </div>;
};

export default HomeLayout;
