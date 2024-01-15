export interface AuthWrapper {
    isAuthenticated: boolean,
    userInfo: AuthenticatedUserInfo | null
  }
  
  export interface AuthenticatedUserInfo {
    userName: string,
    isAdmin: boolean,
    token: string,
    refreshToken: string,
    idToken: string,
    email: string
  }