import { Component, OnInit } from '@angular/core';
import { BookService } from '../../services/book/book.service';
import { Book } from '../../models/book.model';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { NotificationService } from '../../services/notifications/notification.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class HomeComponent implements OnInit {
  books: Book[] = [];

  constructor(
    private bookService: BookService,
    private router: Router,
    private notify: NotificationService 
  ) {}

  navigateToCreate() {
    this.router.navigate(['/book-form']);
  }

  ngOnInit(): void {
    this.bookService.getAllBooks().subscribe({
      next: books => this.books = books,
      error: err => console.error('Kunde inte hämta böcker:', err)
    });
  }

  OnEdit(book: Book) {
    console.log('Redigera', book);
  }

  OnDelete(id: number) {
    this.bookService.removeBook(id).subscribe({
      next: () => {
        this.books = this.books.filter(book => book.id !== id);
        this.notify.show('Bok raderad', 'danger');
      },
      error: err => {
        console.error('Kunde inte ta bort bok:', err);
        this.notify.show('Kunde inte ta bort bok', 'danger');
      }
    });
  }
}


