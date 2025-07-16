import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [FormsModule],
})
export class LoginComponent {
  email: string = '';
  password: string = '';

  constructor(private auth: AuthService, private router: Router) {}

  onSubmit() {
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: () => {
        this.router.navigateByUrl('/');
      },
      error: err => {
        console.error('Inloggning misslyckades', err);
        alert('Fel användaruppgifter');
      }
    });
  }
}
