import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { User } from '../model/User';

const URL = process.env['services__beacon.api__1'];

export const beaconApiSlice = createApi({
    reducerPath: 'beaconApi',
    baseQuery: fetchBaseQuery({ baseUrl: URL }),
    endpoints: (builder) => ({
        getUsers: builder.query<User[], void>({
            query: () => `users`
        })
    })
});

export const { 
    useGetUsersQuery
} = beaconApiSlice;