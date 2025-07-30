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
  template: `
    <div class="container mt-4">
      <div class="d-flex justify-content-between align-items-center mb-3">
        <h2 class="text-success m-0">💬 Mina favoritcitat</h2>
        <button class="btn btn-success btn-lg" (click)="navigateToCreateQuote()">
          <i class="fa fa-plus me-2"></i> Lägg till nytt citat
        </button>
      </div>

      <div class="table-responsive">
        <table class="table table-bordered table-striped">
          <thead class="table-dark">
            <tr>
              <th>Citat</th>
              <th>Författare</th>
              <th class="text-center">Åtgärder</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let quote of quotes">
              <td>{{ quote.text }}</td>
              <td>{{ quote.author }}</td>
              <td class="text-center">
                <ng-container *ngIf="!quote.isStatic">
                  <button class="btn btn-warning btn-sm me-2" (click)="onEditQuote(quote.id!)">
                    <i class="fa fa-edit"></i> Redigera
                  </button>
                  <button class="btn btn-danger btn-sm" (click)="onDeleteQuote(quote.id!)">
                    <i class="fas fa-trash-alt"></i> Ta bort
                  </button>
                </ng-container>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  `
})
export class QuotesListComponent implements OnInit {
  quotes: Quote[] = [];

  readonly staticQuotes: Quote[] = [
    { id: 2001, text: 'Varje dag är en ny chans att börja om.', author: 'Oprah Winfrey', isStatic: true },
    { id: 2002, text: 'Du är starkare än du tror.', author: 'Walt Disney', isStatic: true },
    { id: 2003, text: 'Små steg leder till stora förändringar.', author: 'Okänd', isStatic: true },
    { id: 2004, text: 'Tänk positivt – det gör skillnad.', author: 'Dalai Lama', isStatic: true },
    { id: 2005, text: 'Tro på dig själv, det är där allt börjar.', author: 'Norman Vincent Peale', isStatic: true }
  ];
  


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
        this.quotes = [...this.staticQuotes, ...(data as Quote[])];
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
