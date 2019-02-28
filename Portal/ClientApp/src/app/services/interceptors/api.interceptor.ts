import { Injectable, Inject } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map, finalize } from 'rxjs/operators';
import { AppStateService } from '../app-state.service';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
    constructor(@Inject('BASE_URL') private baseUrl: string, private appStateService: AppStateService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let url = request.url;
        if (!url.startsWith('http')) {
            url = `${this.baseUrl}api/${url}`;
        }

        let headers: {
            [name: string]: string;
        } = {};

        if (!request.headers.has('content-type')) {
            headers['content-type'] = 'application/json; charset=utf-8';
        }

        request = request.clone({
            url: url,
            setHeaders: headers
        });

        this.appStateService.incrementRequests();
        return next.handle(request).pipe(finalize(() => this.appStateService.decrementRequests()));
    }
}
