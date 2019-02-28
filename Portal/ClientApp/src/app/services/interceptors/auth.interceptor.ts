import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { AppStateService } from '../app-state.service';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private appStateService: AppStateService, private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let authHeader = this.appStateService.getAuthHeader();

    if (!!authHeader) {
      request = request.clone({
        setHeaders: {
          [authHeader.name]: authHeader.value
        }
      });
    }

    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.status === 401) {
          this.router.navigate(['/login'], { queryParams: { returnUrl: request.url } });
        }

        return throwError(err);
      })
    );
  }
}
