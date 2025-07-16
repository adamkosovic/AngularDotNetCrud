
import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { QuoteComponent } from './pages/quote/quote.component';
import { AuthGuard } from './guards/auth.guard';
import { BookFormComponent } from './pages/book-form/book-form.component';

export const routes: Routes = [
  { path: '', component: HomeComponent /*, canActivate: [AuthGuard] */ },
  { path: 'login', component: LoginComponent },
  { path: 'quotes', component: QuoteComponent /*, canActivate: [AuthGuard] */ },
  { path: 'book-form', component: BookFormComponent /*, canActivate: [AuthGuard] */ },
  { path: 'book-form/:id', component: BookFormComponent /*, canActivate: [AuthGuard] */ },
  { path: '**', redirectTo: '' },
];

