import { FC } from "react";
import Services from "../components/Services";
import ProductTabs from "../components/ProductTabs";
import ClientReviews from "../components/ClientReviews";
import ShopNow from "../components/ShopNow";
import HomeBlog from "../components/HomeBlog";

const HomePage: FC = () => {
    return <div className="flex flex-col gap-y-16">
        <ProductTabs />
        <Services />
        <ShopNow />
        <ClientReviews />
        <HomeBlog />
    </div>
};

export default HomePage;
