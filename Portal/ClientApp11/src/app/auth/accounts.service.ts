import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface IUserRegistrationModel {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

@Injectable({
  providedIn: 'root'
})
export class AccountsService {
  constructor(private httpClient: HttpClient) { }

  register(model: IUserRegistrationModel): Observable<any> {
    return this.httpClient.post<any>('accounts', model);
  }
}
