
import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { BookFormComponent } from './pages/book-form/book-form.component';
import { QuoteComponent } from './pages/quote/quote.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [authGuard] },
  { path: 'quotes', component: QuoteComponent, canActivate: [authGuard] },
  { path: 'book-form', component: BookFormComponent, canActivate: [authGuard] },
  { path: 'book-form/:id', component: BookFormComponent, canActivate: [authGuard] },

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: '**', redirectTo: '' },
];


