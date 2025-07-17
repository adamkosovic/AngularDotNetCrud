import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  public toasts: { message: string; type: 'success' | 'danger' | 'info' | 'warning' }[] = [];

  show(message: string, type: 'success' | 'danger' | 'info' | 'warning' = 'info') {
    this.toasts.push({ message, type });
    setTimeout(() => this.toasts.shift(), 4000); 
  }

  clear() {
    this.toasts = [];
  }
}
