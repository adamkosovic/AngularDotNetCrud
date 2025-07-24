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
  ) {}

  ngOnInit(): void {
    this.loadQuotes();
  }

  loadQuotes(): void {
    this.quotesService.getQuotes().subscribe({
      next: (data) => (this.quotes = data as Quote[]),
      error: () => this.notificationService.show('Kunde inte ladda citat.')
    });
  }

  navigateToCreateQuote(): void {
    this.router.navigate(['/add-quote']);
  }

  onEditQuote(id: number): void {
    this.router.navigate(['/quote-form', id]);
  }

  onDeleteQuote(id: number): void {
    if (confirm('Är du säker på att du vill ta bort detta citat?')) {
      this.quotesService.deleteQuote(id).subscribe({
        next: () => {
          this.notificationService.show('Citatet togs bort.');
          this.loadQuotes();
        },
        error: () => this.notificationService.show('Kunde inte ta bort citatet.')
      });
    }
  }
}
