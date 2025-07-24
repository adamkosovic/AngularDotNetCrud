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
  ) {
    console.log('🎯 AddQuoteComponent constructor called');
  }

  onSubmit() {
    console.log('🎯 Submitting quote:', this.quote);
    this.quotesService.addQuote(this.quote).subscribe({
      next: () => {
        console.log('🎯 Quote added successfully');
        this.notify.show('Citat tillagt', 'success');
        this.router.navigate(['/quotes']);
      },
      error: (err: any) => {
        console.error('🎯 Error adding quote:', err);
        this.notify.show('Kunde inte skapa citatet', 'danger');
      }
    });
  }
} 