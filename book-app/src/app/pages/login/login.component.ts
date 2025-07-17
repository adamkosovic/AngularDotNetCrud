import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/auth/auth.service';
import { NotificationService } from '../../services/notifications/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [FormsModule, RouterModule],
})
export class LoginComponent implements OnInit {
  email: string = '';
  password: string = '';

  constructor(
    private auth: AuthService,
    private router: Router,
    private notify: NotificationService
  ) {}

  ngOnInit() {
    if (this.auth.isLoggedIn()) {
      this.router.navigateByUrl('/');
    }
  }

  onSubmit() {
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: () => {
        this.notify.show('Inloggning lyckades', 'success');
        setTimeout(() => {
          this.router.navigateByUrl('/');
        }, 100);
      },
      error: err => {
        console.error('Inloggning misslyckades', err);
        alert('Fel användaruppgifter');
        this.notify.show('Inloggning misslyckades', 'danger');
      }
    });
  }
}

