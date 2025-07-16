import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5020'; 

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
    return this.http.post(`${this.apiUrl}/login`, credentials).pipe(
      tap((res: any) => {
        localStorage.setItem('token', res.accessToken); 
      })
    );
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

