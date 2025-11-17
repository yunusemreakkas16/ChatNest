import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './authenticate/services/auth.service';
import { Observable, of, switchMap } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient, private auth: AuthService) { }

  get<T>(url: string): Observable<T> {
    return this.http.get<T>(`${environment.apiUrl}${url}`);
  }

  post<T>(url: string, body: any): Observable<T> {
    return this.http.post<T>(`${environment.apiUrl}${url}`, body);
  }
}
