import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { QuotesService } from '../../services/quotes/quotes.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../services/notifications/notification.service';

@Component({
  selector: 'app-add-quote',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: "./add-quote.component.html"
})
export class AddQuoteComponent {
  quote = {
    text: '',
    author: ''
  };

  constructor(
    private quotesService: QuotesService,
    private router: Router,
    private notify: NotificationService
  ) {}

  onSubmit() {
    this.quotesService.addQuote(this.quote).subscribe({
      next: () => {
        this.notify.show('Citat tillagt', 'success');
        this.router.navigate(['/quotes']);
      },
      error: (err: any) => {
        console.error('Kunde inte skapa citatet:', err);
        this.notify.show('Kunde inte skapa citatet', 'danger');
      }
    });
  }
} 