import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { combineLatest, BehaviorSubject, Observable } from 'rxjs/index';
import { map } from 'rxjs/operators';

export class AuthHeader {
  constructor(public name: string, public value: string) { }
}

interface IToken { id: string; auth_token: string; expires: any; };

export interface IAuthInfo {
  userName: string;
  token: string;
};

const AUTH_INFO = 'auth_info';

@Injectable()
export class AppStateService {
  private _authInfo = new BehaviorSubject<IAuthInfo>(null);

  get isAuthenticated$(): Observable<boolean> {
    return this._authInfo.pipe(map(info => !!info));
  }

  get isAuthenticated(): boolean {
    return !!this._authInfo.value;
  }

  get userName$(): Observable<string> {
    return this._authInfo.pipe(map(info => !!info && info.userName));
  }

  constructor(private router: Router) {
    let authInfo: IAuthInfo = JSON.parse(localStorage.getItem(AUTH_INFO));
    if (!!authInfo) {
      this._authInfo.next(authInfo);
    }
  }

  setAuthenticated(authToken: IAuthInfo) {
    localStorage.setItem(AUTH_INFO, JSON.stringify(authToken));
    this._authInfo.next(authToken);
  }

  getAuthHeader(): AuthHeader {
    if (this.isAuthenticated && !!this._authInfo.value) {
      let token: IToken = JSON.parse(this._authInfo.value.token);
      return new AuthHeader('Authorization', `Bearer ${token.auth_token}`);
    }

    return null;
  }

  logout() {
    if (this.isAuthenticated) {
      this.clearAuthToken();
    }
  }

  private clearAuthToken() {
    this._authInfo.next(null);
    localStorage.removeItem(AUTH_INFO);
  }
}
