import { FC, useEffect, useState } from "react";
import { MessageResource, UserContactResource } from "../../../resources";
import { Avatar } from "antd";
import images from "../../../assets";
import { formatTimeTypeAgo } from "../../../utils/format";
import { selectAuth } from "../../../feature/auth/authSlice";
import { useSelector } from "react-redux";
import Connector from '../../../app/signalR/signalr-connection'

type ChatUserItemProps = {
    group: UserContactResource
    onClick: () => void
}

const ChatUserItem: FC<ChatUserItemProps> = ({
    group,
    onClick
}) => {
    const { events } = Connector();
    const [message, setMessage] = useState<MessageResource>(group.message)
    const { user } = useSelector(selectAuth)

    useEffect(() => {
        events((message) => {
            if(message.sender.id === group.user.id || message.sender.id === user?.id) {
                setMessage(message)
            }
        });
    }, []);

    return <div onClick={onClick} className="cursor-pointer flex items-center gap-x-2 hover:bg-gray-100 p-2 rounded-md">
        <div className="relative">
            <Avatar
                src={images.demoMenth}
            />
            {group.user.isOnline && <span className="absolute bottom-0 right-1 w-[12px] h-[12px] rounded-full border-2 border-white bg-green-500"></span>}
        </div>

        <div className="flex flex-col flex-1">
            <b>{group.user.name}</b>
            <div className="flex items-center justify-between text-[14px] gap-x-2">
                <p className="w-32 truncate">{message.content}</p>
                <span className="text-sky-600">{message.sentAt ? formatTimeTypeAgo(new Date(message.sentAt)) : 'Chưa kết nối'}</span>
            </div>

        </div>
    </div>
};

export default ChatUserItem;