import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgModel, FormsModule } from '@angular/forms';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  standalone: true,
  imports: [FormsModule]
})
export class RegisterComponent {
  email = '';
  password = '';

  constructor(private http: HttpClient, private router: Router, private auth: AuthService) {}

  onRegister() {
    this.http.post('http://localhost:5020/register', {
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        this.auth.login({ email: this.email, password: this.password }).subscribe({
          next: () => {
            this.router.navigate(['/']);
          },
          error: () => {
            alert('Registrering lyckades men inloggning misslyckades');
          }
        });
      },
      error: err => {
        alert('Registrering misslyckades');
        console.error(err);
      }
    });
  }
}
