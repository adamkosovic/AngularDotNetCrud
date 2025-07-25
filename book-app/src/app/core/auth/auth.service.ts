import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUserInfo(): any | null {
    const token = this.getToken();
    if (!token) return null;
  
    const payload = token.split('.')[1];
    try {
      return JSON.parse(atob(payload)); 
    } catch {
      return null;
    }
  }

  login(credentials: { email: string, password: string }) {
    return this.http.post(`${this.apiUrl}/login`, credentials, { withCredentials: true }).pipe(
      tap((res: any) => {
        localStorage.setItem('token', res.accessToken); 
      })
    );
  }

  register(userData: { email: string, password: string }) {
    return this.http.post(`${this.apiUrl}/register`, userData, { withCredentials: true });
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}

