import { RouterProvider } from "react-router-dom";
import OneSignal from 'react-onesignal';
import "./App.css";
import router from "./router";
import Connector from './app/signalR/signalr-connection'
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { initialize, selectAuth } from "./feature/auth/authSlice";
import { getAccessToken } from "./utils/auth";
import { AppDispatch } from "./app/store";
import accountService from "./services/user-service";

function App() {
    const { user } = useSelector(selectAuth)
    const dispatch = useDispatch<AppDispatch>();

    const [enable, setEnable] = useState(false);

    useEffect(() => {
        const init = async () => {
            try {
                if (!enable) {
                    await OneSignal.init({
                        appId: import.meta.env.VITE_ONESIGNAL_APP_ID,
                        autoResubscribe: true,
                        autoRegister: true,
                    });
                }

                await OneSignal.Notifications.requestPermission();
                console.log(OneSignal.Notifications.permission)
                setEnable(OneSignal.Notifications.permission);
                await OneSignal.Slidedown.promptPush();
                OneSignal.Debug.setLogLevel('none');

            } catch (error) {
                console.log('error :>> ', error);
            }
        };

        init();
    }, []);

    useEffect(() => {
        const checkUser = async () => {
            if (user) {
                console.log('Login')
                await OneSignal.login(user.id);
            }
            else if (!user) {
                console.log('Logout')
                await OneSignal.logout();
            }
        }
        if (enable) {
            checkUser()
        }
    }, [user, enable])

    useEffect(() => {
        (async () => {
            const accessToken = getAccessToken();
            if (!accessToken) {
                return dispatch(initialize({
                    isAuthenticated: false, user: undefined
                }));
            }
            try {
                const response = await accountService.getUserDetails();
                if (response.success) {
                    dispatch(initialize({ isAuthenticated: true, user: response.data }));
                } else {
                    dispatch(initialize({
                        isAuthenticated: false, user: undefined
                    }));
                }

            } catch {
                dispatch(initialize({ isAuthenticated: false, user: undefined }));
            }
        })();

    }, []);

    useEffect(() => {
        if (user)
            Connector();
    }, [user])

    return <RouterProvider router={router} />
}

export default App;
