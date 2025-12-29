import { Injectable } from '@angular/core';
import { ApiService } from '../../api.service';
import { CreateUserRequestDto, UpdateUserRequestDto, UserParamModel, UserResponseModel, UserResponseModelDetailed } from '../models/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private apiService: ApiService) {}

  private baseUrl: string = '/api/User';

  AddUser(createUserRequestDto: CreateUserRequestDto): Observable<UserResponseModel> {
    return this.apiService.post(`${this.baseUrl}/AddUser`, createUserRequestDto);
  }

  GetUserDetails(userParam: UserParamModel): Observable<UserResponseModelDetailed> {
    return this.apiService.post(`${this.baseUrl}/UserDetail`, userParam);
  }

  UpdateUser(updateUserRequest: UpdateUserRequestDto): Observable<UserResponseModelDetailed> {
    return this.apiService.post(`${this.baseUrl}/UpdateUser`, updateUserRequest);
  }

  SoftDeleteUser(userParam: UserParamModel): Observable<UserResponseModelDetailed> {
    return this.apiService.post(`${this.baseUrl}/SoftDeleteUser`, userParam);
  }

  ReactivateUser(userParam: UserParamModel): Observable<UserResponseModelDetailed> {
    return this.apiService.post(`${this.baseUrl}/ReActivateUser`, userParam);
  }
}