import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface Quote {
  id: number;
  text: string;
  author: string;
}

@Injectable({
  providedIn: 'root'
})
export class QuotesService {
  private apiUrl = environment.apiUrl + '/api/quotes'; 

  constructor(private http: HttpClient) {}

  getQuotes(): Observable<Quote[]> {
    return this.http.get<Quote[]>(`${this.apiUrl}/quotes`);
  }


  getQuoteById(id: number): Observable<Quote> {
    return this.http.get<Quote>(`${this.apiUrl}/quote/${id}`);
  }

  addQuote(quote: { text: string; author: string }): Observable<Quote> {
    return this.http.post<Quote>(`${this.apiUrl}/quote`, quote);
  }

  updateQuote(id: number, quote: { text: string; author: string }): Observable<Quote> {
    return this.http.put<Quote>(`${this.apiUrl}/quote/${id}`, quote);
  }

  deleteQuote(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/quote/${id}`);
  }
}
