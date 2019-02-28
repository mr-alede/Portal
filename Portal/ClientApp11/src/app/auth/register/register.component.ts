import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap, map, finalize } from 'rxjs/operators';

import { AccountsService, IUserRegistrationModel } from '../accounts.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;

  constructor(private accountsService: AccountsService, private router: Router) { }

  ngOnInit() {
  }

  registerUser({ value, valid }: { value: IUserRegistrationModel, valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';

    if (valid) {
      this.accountsService.register(value)
        .pipe(finalize(() => this.isRequesting = false))
        .subscribe(result => {
          console.log('result', result)
          this.router.navigate(['/login'], { queryParams: { brandNew: true, email: value.email } });
        },
          err => {
            console.log(err);
            this.errors = err.error;
          });
    }
  }
}
