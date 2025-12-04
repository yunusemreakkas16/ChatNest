import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, of, switchMap, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  setTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
  }

  ValidateToken(): Observable<any> {
    const token = this.getAccessToken();
    if (!token) return throwError(() => new Error('No token found'));
    return this.http.post(`${environment.apiUrl}/api/Auth/ValidateToken`, {
      token,
      userAgent: navigator.userAgent,
    });
  }
  
  // 🟢 Modified Section
  refreshToken(): Observable<any> {
    const refresh = this.getRefreshToken();
    if (!refresh) {
      return throwError(() => 'No refresh token');
    }

    // 🟢 Backend's Expectation Format
    const refreshRequest = {
      RefreshToken: refresh
      // UserAgent and other fields can be added if required by backend
    };

    return this.http.post<any>(`${environment.apiUrl}/api/Auth/RefreshToken`, refreshRequest)
      .pipe(
        switchMap(res => {
          // 🟢 added response control
          if (res && res.accessToken && res.refreshToken) {
            this.setTokens(res.accessToken, res.refreshToken);
            return of(res); // 🟢 only setting tokens, returning response if needed
          } else {
            return throwError(() => 'Invalid token response');
          }
        }),
        catchError(error => {
        // 🟢 Show Failure Details
            console.error('Refresh token error details:', {
            status: error.status,
            statusText: error.statusText,
            url: error.url,
            error: error.error,
            message: error.message
            });
            
            this.setTokens('', '');
            return throwError(() => error);
        })
      );
  }

  login(credentials: { email: string; password: string }): Observable<any> {
    return this.http.post(`${environment.apiUrl}/api/Auth/login`, credentials);
  }

  register(data: { Username: string; Email: string; PasswordHash: string }): Observable<any> {
    return this.http.post(`${environment.apiUrl}/api/User/AddUser`, data);
  }

  getUserID(): string {
    const token = localStorage.getItem('accessToken');
    if (!token) return '';

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.sub; // sub = userId
    } catch (e) {
      console.error('Token could not resolved!', e);
      return '';
    }
  }
  
  getUserRole(): string {
    const token = localStorage.getItem('accessToken');
    if (!token) return '';

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    } catch (e) {
      console.error('Token could not resolved!', e);
      return '';
    }
  }
}