import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/auth/auth.service';
import { NotificationService } from '../../services/notifications/notification.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  standalone: true,
  imports: [FormsModule, RouterModule]
})
export class RegisterComponent {
  email = '';
  password = '';

  constructor(
    private http: HttpClient,
    private router: Router,
    private auth: AuthService,
    private notify: NotificationService
  ) {}

  onRegister() {
    this.http.post('http://localhost:5020/register', {
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        this.notify.show('Registrering lyckades', 'success');

        this.auth.login({ email: this.email, password: this.password }).subscribe({
          next: () => {
            this.notify.show('Inloggning lyckades', 'success');
            this.router.navigate(['/']);
          },
          error: () => {
            this.notify.show('Inloggning misslyckades', 'danger');
          }
        });
      },
      error: err => {
        console.error('Registrering misslyckades', err);
        this.notify.show('Registrering misslyckades', 'danger');
      }
    });
  }
}
