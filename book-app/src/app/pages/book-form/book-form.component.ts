import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
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
export class BookFormComponent implements OnInit {
  book: Book = {
    title: '',
    author: '',
    publishDate: ''
  };

  editing = false;
  bookId?: number;


  constructor(
    private bookService: BookService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  
  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.editing = true;
      this.bookId = +id;
      this.bookService.getBookById(+id).subscribe({ 
        next: (book: Book) => this.book = book,
        error: (err: any) => console.error('Kunde inte hämta boken:', err)
      });
    }
  }

  onSubmit() {
    if (this.editing && this.bookId) {
      this.bookService.updateBook(this.bookId, this.book).subscribe({ 
        next: () => this.router.navigate(['/']),
        error: (err: any) => console.error('Kunde inte uppdatera boken:', err)
      });
    } else {
      this.bookService.createBook(this.book).subscribe({
        next: () => this.router.navigate(['/']),
        error: (err: any) => console.error('Kunde inte skapa boken:', err)
      });
    }
  }
}
