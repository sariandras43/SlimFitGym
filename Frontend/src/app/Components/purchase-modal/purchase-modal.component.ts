import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-purchase-modal',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './purchase-modal.component.html',
  styleUrl: './purchase-modal.component.scss'
})
export class PurchaseModalComponent {
  @Input() isOpen = false;
  @Output() cancel = new EventEmitter<void>();
  @Output() modalSubmitted = new EventEmitter<void>();

  cardData = {
    number: '',
    expiry: '',
    cvc: '',
    name: ''
  };

  errorMessage = '';

  onSubmit() {
    if (!this.validateCard()) return;

    this.modalSubmitted.emit();
    this.clearForm();
  }

  private validateCard(): boolean {
    const errors = [];
    const cleanedNumber = this.cardData.number.replace(/\D/g, '');

    // Card number validation
    if (cleanedNumber.length !== 16 || !/^4[0-9]{15}$/.test(cleanedNumber)) {
      errors.push('Hibás kártyaszám');
    }

    // Expiration date validation
    if (!/^(0[1-9]|1[0-2])\/\d{2}$/.test(this.cardData.expiry)) {
      errors.push('Hibás lejárati dátum');
    } else {
      const [month, year] = this.cardData.expiry.split('/');
      const currentYear = new Date().getFullYear() % 100;
      const currentMonth = new Date().getMonth() + 1;
      
      if (parseInt(year) < currentYear || 
          (parseInt(year) === currentYear && parseInt(month) < currentMonth)) {
        errors.push('Lejárt a kártyája');
      }
    }

    // CVC validation
    if (!/^\d{3,4}$/.test(this.cardData.cvc)) {
      errors.push('Hibás CVC');
    }

    // Name validation
    if (!this.cardData.name.trim()) {
      errors.push('Kérem adja meg a kártyatulajdonos nevét');
    }
    if (errors.length > 0) {
      
      this.errorMessage = errors.join(', ');
      return false;
    }
    return true;
  }

  onCancel() {
    this.cancel.emit();
    this.clearForm();
  }

  private clearForm() {
    this.cardData = { number: '', expiry: '', cvc: '', name: '' };
    this.errorMessage = '';
  }

  formatCardNumber(event: Event) {
    let value = (event.target as HTMLInputElement).value.replace(/\D/g, '');
    value = value.replace(/(\d{4})(?=\d)/g, '$1 ');
    this.cardData.number = value.slice(0, 19);
  }

  formatExpiry(event: Event) {
    let value = (event.target as HTMLInputElement).value.replace(/\D/g, '');
    if (value.length > 2) {
      value = value.slice(0, 2) + '/' + value.slice(2, 4);
    }
    this.cardData.expiry = value.slice(0, 5);
  }

  fillTestCredentials() {
    this.cardData = {
      number: '4242 4242 4242 4242',
      expiry: '12/34',
      cvc: '123',
      name: 'Test User'
    };
    this.errorMessage = '';
  }
}