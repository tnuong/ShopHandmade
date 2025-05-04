import { Skeleton } from "antd";
import { FC } from "react";

const GridProductSkeleton: FC = () => {
    return <div className="w-full grid md:grid-cols-3 grid-cols-2 lg:grid-cols-4 gap-8">
        <div className="flex flex-col gap-y-4">
            <Skeleton.Button size="large" style={{
                height: 250
            }} block active />

            <Skeleton paragraph={{ rows: 2 }} />
        </div>
        <div className="flex flex-col gap-y-4">
            <Skeleton.Button size="large" style={{
                height: 250
            }} block active />

            <Skeleton paragraph={{ rows: 2 }} />
        </div>
        <div className="flex flex-col gap-y-4">
            <Skeleton.Button size="large" style={{
                height: 250
            }} block active />

            <Skeleton paragraph={{ rows: 2 }} />
        </div>
        <div className="flex flex-col gap-y-4">
            <Skeleton.Button size="large" style={{
                height: 250
            }} block active />

            <Skeleton paragraph={{ rows: 2 }} />
        </div>
        <div className="flex flex-col gap-y-4">
            <Skeleton.Button size="large" style={{
                height: 250
            }} block active />

            <Skeleton paragraph={{ rows: 2 }} />
        </div>
        <div className="flex flex-col gap-y-4">
            <Skeleton.Button size="large" style={{
                height: 250
            }} block active />

            <Skeleton paragraph={{ rows: 2 }} />
        </div>
        <div className="flex flex-col gap-y-4">
            <Skeleton.Button size="large" style={{
                height: 250
            }} block active />

            <Skeleton paragraph={{ rows: 2 }} />
        </div>
        <div className="flex flex-col gap-y-4">
            <Skeleton.Button size="large" style={{
                height: 250
            }} block active />

            <Skeleton paragraph={{ rows: 2 }} />
        </div>
      
    </div>
};

export default GridProductSkeleton;
