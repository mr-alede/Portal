import { NgModule, ModuleWithProviders, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';

import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { ApiNotificationsInterceptor } from './interceptors/api-notifications.interceptor';
import { AppStateService } from './app-state.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ToastrModule
  ]
})
export class ServicesModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: ServicesModule,
      providers: [
        {
          provide: HTTP_INTERCEPTORS,
          useClass: AuthInterceptor,
          multi: true,
        },
        {
          provide: HTTP_INTERCEPTORS,
          useClass: ApiInterceptor,
          multi: true,
        },
        {
          provide: HTTP_INTERCEPTORS,
          useClass: ApiNotificationsInterceptor,
          multi: true,
        },

        AppStateService
      ]
    };
  }

  constructor(@Optional() @SkipSelf() parentModule: ServicesModule) {
    if (parentModule) {
      throw new Error(
        'ServicesModule is already loaded. Import it in the AppModule only');
    }
  }
}
