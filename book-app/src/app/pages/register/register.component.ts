import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  user = {
    email: '',
    password: '',
    confirmPassword: ''
  };

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  validatePassword(password: string): string[] {
    const errors: string[] = [];
    
    if (!/[A-Z]/.test(password)) {
      errors.push('Lösenordet måste innehålla minst en stor bokstav (A-Z)');
    }
    if (!/[a-z]/.test(password)) {
      errors.push('Lösenordet måste innehålla minst en liten bokstav (a-z)');
    }
    if (!/[0-9]/.test(password)) {
      errors.push('Lösenordet måste innehålla minst en siffra (0-9)');
    }
    if (!/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
      errors.push('Lösenordet måste innehålla minst ett specialtecken (!@#$%^&*)');
    }
    if (password.length < 6) {
      errors.push('Lösenordet måste vara minst 6 tecken långt');
    }
    
    return errors;
  }

  onRegister() {
    if (this.user.password !== this.user.confirmPassword) {
      alert('Lösenorden matchar inte!');
      return;
    }

    const passwordErrors = this.validatePassword(this.user.password);
    if (passwordErrors.length > 0) {
      alert('Lösenordet uppfyller inte kraven:\n' + passwordErrors.join('\n'));
      return;
    }

    this.http.post(`${environment.apiUrl}/register`, {
      email: this.user.email,
      password: this.user.password
    }).subscribe({
      next: (response) => {
        console.log('Registrering lyckades', response);
        alert('Registrering lyckades! Du kan nu logga in.');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Registrering misslyckades', error);
        if (error.error?.errors) {
          const errorMessages = Object.values(error.error.errors).flat();
          alert('Registrering misslyckades:\n' + errorMessages.join('\n'));
        } else {
          alert('Registrering misslyckades. Försök igen.');
        }
      }
    });
  }
}
