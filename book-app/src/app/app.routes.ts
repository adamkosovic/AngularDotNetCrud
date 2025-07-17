
import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { BookFormComponent } from './pages/book-form/book-form.component';
import { RegisterComponent } from './pages/register/register.component';
import { QuoteComponent } from './pages/quote/quote.component';

export const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'quotes', component: QuoteComponent, canActivate: [AuthGuard] },
  { path: 'book-form', component: BookFormComponent, canActivate: [AuthGuard] },
  { path: 'book-form/:id', component: BookFormComponent, canActivate: [AuthGuard] },

  { path: 'login', component: LoginComponent }, 
  { path: 'register', component: RegisterComponent },

  { path: '**', redirectTo: '' },
];

