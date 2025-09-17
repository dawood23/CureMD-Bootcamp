export interface LoginResponse{
    success:boolean,
    token:string|null,
    message:string,
    refreshToken:string
}