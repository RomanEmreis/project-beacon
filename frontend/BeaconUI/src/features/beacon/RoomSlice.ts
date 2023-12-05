import { PayloadAction, createSelector, createSlice } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";

interface State {
    currentUserName: string;
};

const initialState: State = {
    currentUserName: ''
};

const slice = createSlice({
	name: 'room',
	initialState: initialState,
	reducers: {
		setUserName: (state, action: PayloadAction<string>) => {
            return { ...state, currentUserName: action.payload };
        }
	},
});

const getSliceRoot = (rootState: RootState) => rootState.room;

export default slice.reducer;
export const selectCurrentUserName = createSelector(getSliceRoot, (state: State) => state.currentUserName);
export const { 
    setUserName
 } = slice.actions;