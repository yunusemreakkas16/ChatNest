import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { AuthService } from "../services/auth.service";
import { Router } from "@angular/router";
import { catchError, Observable, switchMap, throwError, BehaviorSubject, filter, take } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  // 🟢 Variables for Resfresh Token Process
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  constructor(private auth: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Adding Authorization header
    if (req.url.includes('/Auth/RefreshToken')) {
      return next.handle(req);
    }

    const token = this.auth.getAccessToken();
    let cloned = req;

    if (token) {
      cloned = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(cloned).pipe(
      catchError((err: HttpErrorResponse) => {
        // Only handle 401 errors that are not from the refresh token endpoint
        if (err.status === 401 && !req.url.includes('/Auth/RefreshToken')) {
          return this.handle401Error(cloned, next);
        }
        return throwError(() => err);
      })
    );
  }

  // 🟢 catches 401 failures
  private handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.auth.refreshToken().pipe(
        switchMap((tokenResponse: any) => {
          this.isRefreshing = false;
          
          const newToken = this.auth.getAccessToken();
          this.refreshTokenSubject.next(newToken);
          
          // 🟢 try new token
          const authReq = request.clone({
            setHeaders: {
              Authorization: `Bearer ${newToken}`
            }
          });
          
          return next.handle(authReq);
        }),
        catchError((refreshError) => {
          this.isRefreshing = false;
          
          // 🟢 refresh token failed try again
          this.auth.setTokens('', '');
          this.router.navigate(['/auth/login']);
          return throwError(() => refreshError);
        })
      );
    } else {
      // 🟢 still refreshing wait till the end
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap(token => {
          const authReq = request.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`
            }
          });
          return next.handle(authReq);
        })
      );
    }
  }
}