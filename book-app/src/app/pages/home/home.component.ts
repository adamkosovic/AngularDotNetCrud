import { Component, OnInit } from '@angular/core';
import { BookService } from '../../services/book.service';
import { Book } from '../../models/book.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { BookFormComponent } from '../book-form/book-form.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  standalone: true,
  imports: [CommonModule, RouterModule]
})


export class HomeComponent implements OnInit {
  books: Book[] = [];

  constructor(private bookService: BookService, private router: Router) {}

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
      },
      error: err => console.error('Kunde inte ta bort bok:', err)
    });
  }
}

