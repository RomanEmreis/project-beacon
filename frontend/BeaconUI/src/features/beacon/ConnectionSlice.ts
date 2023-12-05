import { PayloadAction, createSelector, createSlice } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";
import { User } from "../../model/User";

interface State {
    isConnected: boolean,
    onlineUsers: User[]
};

const initialState: State = {
    isConnected: false,
    onlineUsers: []
};

const slice = createSlice({
	name: 'connection',
	initialState: initialState,
	reducers: {
        setIsConnected: (state, action: PayloadAction<boolean>) => {
            return { ...state, isConnected: action.payload };
        },
		updateUserList: (state, action: PayloadAction<User[]>) => {
            return { ...state, onlineUsers: action.payload };
        },
        callAccepted: (state, action: PayloadAction<User>) => {
            console.log(action.payload);
        },
        callDeclined: (state, action: PayloadAction<{ decliningUser: User, reason: any }>) => {
            console.log(action.payload);
        },
        incomingCall: (state, action: PayloadAction<User>) => {
            console.log(action.payload);
        },
        receiveSignal: (state, action: PayloadAction<{ signalingUser: User, signal: any }>) => {
            console.log(action.payload);
        },
        callEnded: (state, action: PayloadAction<{ signalingUser: User, signal: any }>) => {
            console.log(action.payload);
        }
	},
});

const getSliceRoot = (rootState: RootState) => rootState.connection;

export default slice.reducer;
export const selectIsConnected = createSelector(getSliceRoot, (state: State) => state.isConnected);
export const selectOnlineUsers = createSelector(getSliceRoot, (state: State) => state.onlineUsers);
export const {
    setIsConnected,
    updateUserList,
    callAccepted,
    callDeclined,
    incomingCall,
    receiveSignal,
    callEnded
 } = slice.actions;