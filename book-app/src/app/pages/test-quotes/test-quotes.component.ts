import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-test-quotes',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div style="background: red; color: white; padding: 20px; margin: 20px; font-size: 24px;">
      🎯 TEST QUOTES COMPONENT IS WORKING!
      <br>
      If you see this red box, routing to quotes is working!
    </div>
  `
})
export class TestQuotesComponent {
  constructor() {
    console.log('🎯 TestQuotesComponent constructor called');
  }
} 