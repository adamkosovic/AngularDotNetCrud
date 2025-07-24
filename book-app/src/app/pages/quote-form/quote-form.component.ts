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
  ) {
    console.log('🎯 QuoteFormComponent constructor called');
  }

  ngOnInit(): void {
    console.log('🎯 QuoteFormComponent ngOnInit called');
    const id = this.route.snapshot.paramMap.get('id');
    console.log('🎯 Route ID parameter:', id);
    if (id) {
      this.editing = true;
      this.quoteId = +id;
      console.log('🎯 Editing quote with ID:', this.quoteId);
      this.quotesService.getQuoteById(this.quoteId).subscribe({
        next: (quote: any) => {
          console.log('🎯 Quote loaded for editing:', quote);
          this.quote = quote;
        },
        error: (err: any) => {
          console.error('🎯 Error loading quote for editing:', err);
          this.notify.show('Kunde inte hämta citatet', 'danger');
        }
      });
    } else {
      console.log('🎯 Creating new quote (no ID provided)');
    }
  }

  onSubmit() {
    console.log('🎯 Submitting quote form:', this.quote);
    if (this.editing && this.quoteId) {
      console.log('🎯 Updating existing quote');
      this.quotesService.updateQuote(this.quoteId, this.quote).subscribe({
        next: () => {
          console.log('🎯 Quote updated successfully');
          this.notify.show('Citat uppdaterat', 'success');
          this.router.navigate(['/quotes']);
        },
        error: (err: any) => {
          console.error('🎯 Error updating quote:', err);
          this.notify.show('Kunde inte uppdatera citatet', 'danger');
        }
      });
    } else {
      console.log('🎯 Creating new quote');
      this.quotesService.addQuote(this.quote).subscribe({
        next: () => {
          console.log('🎯 Quote created successfully');
          this.notify.show('Citat tillagt', 'success');
          this.router.navigate(['/quotes']);
        },
        error: (err: any) => {
          console.error('🎯 Error creating quote:', err);
          this.notify.show('Kunde inte skapa citatet', 'danger');
        }
      });
    }
  }
} 