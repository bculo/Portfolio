export type UserType = "Admin" | "User";

export interface UserAuthenticated {
    email: string;
    username: string;
    role: UserType;
}