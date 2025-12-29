import { BaseResponse } from "src/app/base-response-model/base-response";

export interface UserParamModel {
  userID: string;
}

export interface UserResponse {
  userID: string;
  userName: string;
  userEmail: string;
  userRole: string;
  createdAt: Date;
}

export interface UserResponseDetailed {
  userID: string;
  userName: string;
  userEmail: string;
  userPasswordHash: string;
  userRole: string;
  createdAt: Date;
}

export interface UserResponseModel extends BaseResponse {
  user?: UserResponse;
}

export interface UserResponseModelDetailed extends BaseResponse {
  user?: UserResponseDetailed;
}

export interface UserResponseModelList extends BaseResponse {
  users?: UserResponse[];
}

export interface UserIDResponse {
  userID: string;
}

export interface UserIDResponseModel extends BaseResponse {
  userIDResponse?: UserIDResponse[];
}

export interface LoginResponseDto extends BaseResponse {
  accessToken?: string;
  refreshToken?: string;
}

export interface LoginRequestDto {
  email: string;
  password: string;
  reactivateIfDeleted?: boolean;
}

export interface CreateUserRequestDto {
  username: string;
  email: string;
  passwordHash: string;
}

export interface UpdateUserRequestDto {
  userID: string;
  username: string;
  email: string;
  passwordHash: string;
}

export interface GetIDsByEmailRequestsDto {
  email: string[];
}