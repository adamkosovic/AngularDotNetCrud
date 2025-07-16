import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BookService } from '../../services/book.service';
import { Book } from '../../models/book.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-book-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './book-form.component.html',
  styleUrl: './book-form.component.css'
})
export class BookFormComponent {
  book: Book = {
    title: '',
    author: '',
    publishDate: ''
  };

  constructor(
    private bookService: BookService,
    private router: Router
  ) {}

  onSubmit() {
    this.bookService.createBook(this.book).subscribe({
      next: () => this.router.navigate(['/']),
      error: err => console.error('Fel vid skapande:', err)
    });
  }
}
