import { Component } from '@angular/core';
import { AuthService } from '../../core/auth/auth.service';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../services/notifications/notification.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  isDarkMode = false;

  constructor(
    public auth: AuthService,
    private router: Router,
    private notify: NotificationService
  ) {}

  toggleTheme() {
    console.log('toggleTheme');

    const body = document.querySelector('body');
    if (!body) return;

    this.isDarkMode = !this.isDarkMode;

    if (this.isDarkMode) {
      body.classList.add('dark-theme');
    } else {
      body.classList.remove('dark-theme');
    }
  }

  logout() {
    this.auth.logout();
    this.notify.show('Utloggning lyckades', 'success');
    this.router.navigate(['/login']);
  }
}

