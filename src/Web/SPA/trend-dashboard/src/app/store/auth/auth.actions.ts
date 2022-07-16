import { createAction, props } from "@ngrx/store";
import { UserAuthenticated } from "./auth.models";

const USER_AUTHENTICATION_STARTED = "[Auth] User authentication started";
const USER_AUTHENTICATED = "[Auth] User authenticated";
const USER_AUTHENTICATION_FAILED = "[Auth] User authentication failed";

export const userAuthenticationStarted = createAction(
    USER_AUTHENTICATION_STARTED,
);

export const userAuthenticated = createAction(
    USER_AUTHENTICATED,
    props<{
        status: UserAuthenticated
    }>()
);

export const userAuthenticationFailed = createAction(
    USER_AUTHENTICATION_FAILED,
);

const USER_LOGOUT = "[Auth] User logout";

export const userLogout = createAction(
    USER_LOGOUT
);

