import { FC, ReactNode, useState } from "react";
import NotificationContext from "./notification-context";
import { NotificationResource } from "../../../resources";


type NotificationProviderProps = {
    children: ReactNode
}

const NotificationProvider: FC<NotificationProviderProps> = ({
    children
}) => {
    const [notification, setNotification] = useState<NotificationResource | null>(null)
    const [count, setCount] = useState(0)
   
    return (
        <NotificationContext.Provider value={{ notification, count }}>
            {children}
        </NotificationContext.Provider>
    );
}

export default NotificationProvider;
