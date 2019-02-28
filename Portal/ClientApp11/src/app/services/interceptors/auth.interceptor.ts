import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse } from '@angular/common/http';
import { AppStateService } from '../app-state.service';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private appStateService: AppStateService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let authHeader = this.appStateService.getAuthHeader();

    if (!!authHeader) {
      request = request.clone({
        setHeaders: {
          [authHeader.name]: authHeader.value
        }
      });
    }

    return next.handle(request);
  }
}
