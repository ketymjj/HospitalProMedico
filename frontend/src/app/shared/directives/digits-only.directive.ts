import { Directive, HostListener, Optional, Self } from '@angular/core';
import { NgControl } from '@angular/forms';

/** Strips any non-digit character as the user types, without adding formatting. */
@Directive({
  selector: '[appDigitsOnly]',
  standalone: true
})
export class DigitsOnlyDirective {
  constructor(@Optional() @Self() private ngControl: NgControl | null) {}

  @HostListener('input', ['$event'])
  onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const digits = input.value.replace(/\D/g, '');

    if (input.value !== digits) {
      this.ngControl?.control?.setValue(digits);
    }
  }
}
