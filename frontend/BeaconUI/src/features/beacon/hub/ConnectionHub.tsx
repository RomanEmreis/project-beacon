import { FC, PropsWithChildren, useCallback, useEffect } from "react";
import SignalRConnector from "../../../utils/signalr/SignalRConnector";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { User } from "../../../model/User";
import { callAccepted, callDeclined, callEnded, incomingCall, receiveSignal, selectIsConnected, setIsConnected, updateUserList } from "../ConnectionSlice";
import { setUserName } from "../RoomSlice";

const ConnectionHub: FC<PropsWithChildren> = ({ children }) => {
    const dispatch = useAppDispatch();
    const isConnected = useAppSelector(selectIsConnected);

    const handleConnected = useCallback(() => {
        dispatch(setIsConnected(true));
    }, [dispatch]);

    const { connection } = SignalRConnector('/ConnectionHub', handleConnected);

    useEffect(() => {
        connection.on('updateUserList', (userList: User[]) => dispatch(updateUserList(userList)));
        connection.on('callAccepted', (acceptingUser: User) => dispatch(callAccepted(acceptingUser)));
        connection.on('callDeclined', (decliningUser: User, reason: any) => dispatch(callDeclined({ decliningUser, reason })));
        connection.on('incomingCall', (callingUser: User) => dispatch(incomingCall(callingUser)));
        connection.on('receiveSignal', (signalingUser: User, signal: any) => dispatch(receiveSignal({ signalingUser, signal })));
        connection.on('callEnded', (signalingUser: User, signal: any) => dispatch(callEnded({ signalingUser, signal })));
    }, [connection]);

    useEffect(() => {
        if (isConnected) {
            const userName = prompt('Select a User Name');
            if (userName) {
                connection.invoke("Join", userName).catch((err) => {
                    console.error(err);
                });
                dispatch(setUserName(userName));
            }
        }
    }, [isConnected]);

    return (<>{children}</>);
};

export default ConnectionHub