import { Component, NgZone } from '@angular/core';
import { Router } from '@angular/router';

import { AppStateService, IAuthInfo } from './services/app-state.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'EtaCom';

  constructor(private router: Router, private appStateService: AppStateService, private zone: NgZone) {
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
}
