import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { QuotesService } from '../../services/quotes/quotes.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../services/notifications/notification.service';

@Component({
  selector: 'app-quote-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './quote-form.component.html'
})
export class QuoteFormComponent implements OnInit {
  quote = {
    text: '',
    author: ''
  };

  editing = false;
  quoteId?: number;

  constructor(
    private quotesService: QuotesService,
    private router: Router,
    private route: ActivatedRoute,
    private notify: NotificationService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.editing = true;
      this.quoteId = +id;
      this.quotesService.getQuoteById(this.quoteId).subscribe({
        next: (quote: any) => this.quote = quote,
        error: (err: any) => {
          console.error('Kunde inte hämta citatet:', err);
          this.notify.show('Kunde inte hämta citatet', 'danger');
        }
      });
    }
  }

  onSubmit() {
    if (this.editing && this.quoteId) {
      this.quotesService.updateQuote(this.quoteId, this.quote).subscribe({
        next: () => {
          this.notify.show('Citat uppdaterat', 'success');
          this.router.navigate(['/quotes']);
        },
        error: (err: any) => {
          console.error('Kunde inte uppdatera citatet:', err);
          this.notify.show('Kunde inte uppdatera citatet', 'danger');
        }
      });
    } else {
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
} 