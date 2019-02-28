import { Component, NgZone, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

import { AppStateService, IAuthInfo } from './services';
import { Observable } from 'rxjs';
import { ToastContainerDirective, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  @ViewChild(ToastContainerDirective) toastContainer: ToastContainerDirective;

  public busy$: Observable<boolean>;

  constructor(
    private router: Router, private appStateService: AppStateService,
    private zone: NgZone, private toastrService: ToastrService) {
    if (!(<any>window).externalProviderLogin) {
      var self = this;
      (<any>window).externalProviderLogin = function (auth: IAuthInfo) {
        self.zone.run(() => {
          self.appStateService.setAuthenticated(auth);
          self.router.navigate(['/']);
        });
      }
    }
  }

  ngOnInit(): void {
    this.toastrService.overlayContainer = this.toastContainer;

    this.busy$ = this.appStateService.busy$;
  }
}
