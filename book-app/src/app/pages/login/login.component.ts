import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [FormsModule, RouterModule],
})
export class LoginComponent {
  email: string = '';
  password: string = '';

  constructor(private auth: AuthService, private router: Router) {}

  onSubmit() {
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: () => {
        // Vänta lite innan navigering så token säkert är sparad
        setTimeout(() => {
          this.router.navigateByUrl('/');
        }, 100);
      },
      error: err => {
        console.error('Inloggning misslyckades', err);
        alert('Fel användaruppgifter');
      }
    });
  }

  ngOnInit() {
    // Omdirigera om redan inloggad
    if (this.auth.isLoggedIn()) {
      this.router.navigateByUrl('/');
    }
  }
}
