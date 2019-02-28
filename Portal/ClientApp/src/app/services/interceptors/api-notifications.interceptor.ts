import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NotificationsService } from '../notifications.service';

@Injectable()
export class ApiNotificationsInterceptor implements HttpInterceptor {
    constructor(private notificationsService: NotificationsService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((err: HttpErrorResponse) => {
                if (err.status === 400 || err.status >= 500) {
                    this.extractErrorMessage(err.error);
                }

                return throwError(err);
            })
        );
    }

    private extractErrorMessage(error: any) {
        if (!error) {
            return;
        }

        let messages = Object.getOwnPropertyNames(error)
            .map(name => error[name])
            .map(msg => {
                if (msg instanceof Array) {
                    return msg[0];
                }

                return msg;
            })
            .filter(x => !!x);

        if (messages && messages.length > 0) {
            console.log('error', messages[0])
            this.notificationsService.error(messages[0]);
        }
    }
}
