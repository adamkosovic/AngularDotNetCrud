
import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { BookFormComponent } from './pages/book-form/book-form.component';
import { TestQuotesComponent } from './pages/test-quotes/test-quotes.component';
import { AddQuoteComponent } from './pages/add-quote/add-quote.component';
import { QuoteFormComponent } from './pages/quote-form/quote-form.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [authGuard] },
  { path: 'quotes', component: TestQuotesComponent, canActivate: [authGuard] },
  { path: 'add-quote', component: AddQuoteComponent, canActivate: [authGuard] },
  { path: 'quote-form/:id', component: QuoteFormComponent, canActivate: [authGuard] },
  { path: 'book-form', component: BookFormComponent, canActivate: [authGuard] },
  { path: 'book-form/:id', component: BookFormComponent, canActivate: [authGuard] },

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: '**', redirectTo: '' },
];


