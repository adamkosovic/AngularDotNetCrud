import { Component } from '@angular/core';
import { AuthService } from '../../core/auth/auth.service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-quote',
  templateUrl: './quote.component.html',
  standalone: true,
  imports: [CommonModule]
})
export class QuoteComponent {
  quotes = [
    "Den som läser lever tusen liv innan han dör.",
    "Att läsa är att resa utan att lämna hemmet.",
    "Böcker är ett fönster till världen.",
    "Kunskap börjar med nyfikenhet.",
    "En bok är en dröm du håller i handen."
  ];

  constructor(private auth: AuthService) {}
} 