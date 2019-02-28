import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap, map } from 'rxjs/operators';
import { AppStateService, IAuthInfo } from '../services/app-state.service';

export interface ICredentials {
  userName: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private httpClient: HttpClient, private appStateService: AppStateService) { }

  login(credentials: ICredentials) {
    return this.httpClient.post<IAuthInfo>('auth/login', credentials)
      .pipe(
        tap(authInfo => {
          this.appStateService.setAuthenticated(authInfo)
        })
      );
  }

  logout() {
    this.appStateService.logout();
  }
}
