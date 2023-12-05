import { configureStore, ThunkAction, Action, combineReducers } from "@reduxjs/toolkit"
import { beaconApiSlice } from "../api/beaconApiSlice"
import connection from '../features/beacon/ConnectionSlice';
import room from '../features/beacon/RoomSlice';

const rootReducer = combineReducers({
    connection: connection,
    room: room,
    [beaconApiSlice.reducerPath]: beaconApiSlice.reducer
});

const middlewares = [
    beaconApiSlice.middleware
]

export const store = configureStore({
    reducer: rootReducer,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(...middlewares)
})

export type AppDispatch = typeof store.dispatch
export type RootState = ReturnType<typeof store.getState>
export type AppThunk<ReturnType = void> = ThunkAction<
    ReturnType,
    RootState,
    unknown,
    Action<string>
>
