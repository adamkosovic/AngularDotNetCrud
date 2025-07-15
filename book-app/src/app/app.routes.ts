
import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { QuoteComponent } from './pages/quote/quote.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent /*, canActivate: [AuthGuard] */ },
  { path: 'login', component: LoginComponent },
  { path: 'quotes', component: QuoteComponent /*, canActivate: [AuthGuard] */ },
  { path: '**', redirectTo: '' },
];

