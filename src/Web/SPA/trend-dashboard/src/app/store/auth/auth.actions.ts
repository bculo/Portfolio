import { createAction, props } from "@ngrx/store";
import { UserAuthenticated } from "./auth.models";

const USER_AUTHENTICATED = "[Auth] User authenticated";

export const userAuthenticated = createAction(
    USER_AUTHENTICATED,
    props<{
        status: UserAuthenticated
    }>()
);

const USER_LOGOUT = "[Auth] User logout";

export const userLogout = createAction(
    USER_LOGOUT
);

