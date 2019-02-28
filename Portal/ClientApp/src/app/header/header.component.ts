import { Component, OnInit } from '@angular/core';
import { AppStateService } from '../services/app-state.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  public isAuthenticated = false;
  public userName = '';

  constructor(private appState: AppStateService, private router: Router) { }

  ngOnInit() {
    this.appState.isAuthenticated$.subscribe(isAuthenticated => this.isAuthenticated = isAuthenticated);
    this.appState.userName$.subscribe(userName => this.userName = userName);
  }

  logout($event) {
    $event.preventDefault();
    this.appState.logout();
    this.router.navigate(['/login']);
  }
}
