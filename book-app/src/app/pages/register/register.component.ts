import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../core/auth/auth.service';

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
    private auth: AuthService,
    private router: Router
  ) {}

  validatePassword(password: string): string[] {
    const errors: string[] = [];
    
    if (!/[A-Z]/.test(password)) {
      errors.push('Lösenordet måste innehålla minst en stor bokstav');
    }
    if (!/[a-z]/.test(password)) {
      errors.push('Lösenordet måste innehålla minst en liten bokstav');
    }
    if (!/\d/.test(password)) {
      errors.push('Lösenordet måste innehålla minst en siffra');
    }
    if (!/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
      errors.push('Lösenordet måste innehålla minst ett specialtecken');
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

    this.auth.register({
      email: this.user.email,
      password: this.user.password
    }).subscribe({
      next: (response: any) => {
        console.log('Registrering lyckades', response);
        console.log('Response type:', typeof response);
        console.log('Response status:', response?.status);
        
        // Identity API returns null body on successful registration (status 200)
        // If we reach this point, registration was successful
        alert('Registrering lyckades! Du loggas in automatiskt.');
        
        // Automatically log in the user after successful registration
        this.auth.login({
          email: this.user.email,
          password: this.user.password
        }).subscribe({
          next: (loginResponse: any) => {
            console.log('Automatisk inloggning lyckades', loginResponse);
            this.router.navigate(['/']); // Navigate to home page
          },
          error: (loginError) => {
            console.error('Automatisk inloggning misslyckades', loginError);
            // If auto-login fails, redirect to login page
            this.router.navigate(['/login']);
          }
        });
      },
      error: (error) => {
        console.error('Registrering misslyckades', error);
        console.error('Error status:', error.status);
        console.error('Error message:', error.message);
        
        if (error.error?.errors) {
          const errorMessages = Object.values(error.error.errors).flat();
          alert('Registrering misslyckades:\n' + errorMessages.join('\n'));
        } else if (error.status === 404) {
          alert('Registrering misslyckades: Endpoint hittades inte. Kontrollera API-anslutningen.');
        } else {
          alert('Registrering misslyckades. Försök igen.');
        }
      }
    });
  }
}
