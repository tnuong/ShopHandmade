import { Avatar } from "antd";
import { FC, useEffect, useState } from "react";
import images from "../../../assets";
import { MessageResource, UserContactResource } from "../../../resources";
import Connector from '../../../app/signalR/signalr-connection'
import { formatTimeTypeAgo } from "../../../utils/format";
import { useSelector } from "react-redux";
import { selectAuth } from "../../../feature/auth/authSlice";

type ChatUserItemProps = {
    group: UserContactResource;
    onClick: () => void
}

const ChatUserItem: FC<ChatUserItemProps> = ({
    group,
    onClick
}) => {

    return <div onClick={onClick} className="cursor-pointer flex items-center gap-x-2 hover:bg-gray-100 p-2 rounded-md">
        <div className="relative">
            <Avatar
                src={images.demoMenth}
                size='large'
            />
            {group.user.isOnline && <span className="absolute bottom-0 right-1 w-[12px] h-[12px] rounded-full border-2 border-white bg-green-500"></span>}
        </div>

        <div className="flex flex-col flex-1">
            <b>{group.user.name}</b>
        </div>
    </div>
};

export default ChatUserItem;
