import { Injectable, Inject } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
    constructor(@Inject('BASE_URL') private baseUrl: string) { }

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

        return next.handle(request);
    }
}
