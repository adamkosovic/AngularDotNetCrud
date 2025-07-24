import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { QuotesService } from '../../../services/quotes/quotes.service';
import { NotificationService } from '../../../services/notifications/notification.service';
import { Quote } from '../../../models/quote.model';

@Component({
  selector: 'app-quotes-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './quotes-list.component.html'
})
export class QuotesListComponent implements OnInit {
  quotes: Quote[] = [];

  constructor(
    private quotesService: QuotesService,
    private notificationService: NotificationService,
    private router: Router
  ) {
    console.log('🎯 QuotesListComponent constructor called');
  }

  ngOnInit(): void {
    console.log('🎯 QuotesListComponent ngOnInit called');
    console.log('🎯 Component selector: app-quotes-list');
    this.loadQuotes();
  }

  loadQuotes(): void {
    console.log('🎯 Loading quotes...');
    this.quotesService.getQuotes().subscribe({
      next: (data) => {
        console.log('🎯 Quotes loaded successfully:', data);
        this.quotes = data as Quote[];
        console.log('🎯 Quotes array updated:', this.quotes);
      },
      error: (error) => {
        console.error('🎯 Error loading quotes:', error);
        this.notificationService.show('Kunde inte ladda citat.', 'danger');
      }
    });
  }

  navigateToCreateQuote(): void {
    console.log('🎯 Navigating to add quote');
    this.router.navigate(['/add-quote']);
  }

  onEditQuote(id: number): void {
    console.log('🎯 Editing quote with id:', id);
    this.router.navigate(['/quote-form', id]);
  }

  onDeleteQuote(id: number): void {
    console.log('🎯 Deleting quote with id:', id);
    if (confirm('Är du säker på att du vill ta bort detta citat?')) {
      this.quotesService.deleteQuote(id).subscribe({
        next: () => {
          this.notificationService.show('Citatet togs bort.', 'success');
          this.loadQuotes();
        },
        error: () => this.notificationService.show('Kunde inte ta bort citatet.', 'danger')
      });
    }
  }
}
