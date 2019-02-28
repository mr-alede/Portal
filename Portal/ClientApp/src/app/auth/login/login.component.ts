import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { ICredentials, AuthService } from '../auth.service';
import { tap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit, OnDestroy {
  private subscription: Subscription;
  externalProviderWindow = null;

  brandNew: boolean;
  errors: string;
  submitted: boolean = false;
  credentials: ICredentials = { userName: '', password: '' };

  constructor(private authService: AuthService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    // subscribe to router event
    this.subscription = this.activatedRoute.queryParams.subscribe(
      (param: any) => {
        this.brandNew = param['brandNew'];
        this.credentials.userName = param['email'];
      });
  }

  ngOnDestroy() {
    // prevent memory leak by unsubscribing
    this.subscription.unsubscribe();
  }

  login({ value, valid }: { value: ICredentials, valid: boolean }) {
    this.submitted = true;
    this.errors = '';
    if (valid) {
      this.authService.login(value)
        .subscribe(
          result => {
            if (result) {
              this.router.navigate(['/']);
            }
          },
          error => this.errors = error);
    }
  }

  callExternalLogin(providerName: string) {
    var url = "externalAuth/Login/" + providerName;
    // minimalistic mobile devices support
    var w = (screen.width >= 1050) ? 1050 : screen.width;
    var h = (screen.height >= 550) ? 550 : screen.height;
    var params = "toolbar=yes,scrollbars=yes,resizable=yes,width=" + w + ", height=" + h;
    // close previously opened windows (if any)
    if (this.externalProviderWindow) {
      this.externalProviderWindow.close();
    }
    this.externalProviderWindow = window.open(url, "ExternalProvider", params, false);
  }
}
